var xPos; // store mouse X position relative to container that is dragging
var yPos; // store mouse Y position relative to container that is dragging
var canvasX; // store X position of container that is dragging
var canvasY; // store Y position of container that is dragging
var poop; // parent container that background canvas element will be appended to
var ProjectObject = {}; // Object that will be passed to Ajax call with Project Info
ProjectObject.ProjectClass = []; // Object will hold IEnumerable of Objects within Array

var backVideo; // store the background video element that is created


// This function is called from the NewProjectDisplay.cshtml view
// It accepts one argument which is the filepath to the backgrounf video
function createBackGroundCanvas(filepath)
{
    // create new element to be drawn to canvas
    backVideo = document.createElement("video"); 

    /*  add event listener to video element BEFORE setting the source
        This allows the browser to listen to the file that is being loaded in
        Once it begins to listen it can use MetaData function 
        which is an event type of the video element,
        It can then retrieve the video file's original width and height
        the video element inherits from HTMLVideoElement 
        which has videoWidth and videoHeight properties
        developer.mozilla.org/en-US/docs/Web/Events/loadedmetadata
        developer.mozilla.org/en-US/docs/Web/API/HTMLVideoElement
    */
    backVideo.addEventListener("loadedmetadata", OnMetaData);
    backVideo.src = filepath; // tell the method which file to reference
    
    // set initial width and height and play() to trigger loadedmetadata event
    backVideo.width = 640;
    backVideo.height = 360;
    backVideo.loop = true;
    backVideo.play();
}

/*  This function receives an event object from the createBackGroundCanvas function
    That object targets the newly created video element 
    it then provides metadata which shows the videos orginal width and height
*/
function OnMetaData(ev)
{
    // parent container that the Canvas element will append to 
    poop = document.getElementById("vid-1");
    
    /*  getBoundingClientRect retrieves the parent container
        has a property of width, the width will be used to set the width of the canvas
    */
    var poopWidth = poop.getBoundingClientRect().width;

    // original Width and height of loaded video retrieved from metadata
    var videoWidth = ev.target.videoWidth;
    var videoHeight = ev.target.videoHeight;

    /*  divide the parent container width by the video original width to get the
        percentage of width for scaling
    */
    var AlteredOriginalWidthInPercent = poopWidth / videoWidth;

    /*  take the event object that was passed in and it's target 
        which is the video element created and set it's width to the width 
        of the parent conatiner
    */
    ev.target.width = poopWidth;


    /*  to get the proper aspect ratio for the height, 
        take the ev.target and set it's height to the 
        metadataHeight * the Altered % and set the height
    */
    ev.target.height = videoHeight * AlteredOriginalWidthInPercent;

    // create an element to draw the background video element to
    var backCanvas = document.createElement("canvas");
    backCanvas.id = "background-video"; // set the ID so the drop function can be added
    backCanvas.width = ev.target.width;
    backCanvas.height = ev.target.height;

    var context = backCanvas.getContext("2d");

    poop.appendChild(backCanvas);

    // listen for video to play to start the drawing to canvas
    backVideo.addEventListener("play", function () {
        var $this = this;// cache
        (function loop() {
            if (!this.paused && !this.ended) {
                context.drawImage(backVideo, 0, 0, backCanvas.width, backCanvas.height);
                setTimeout(loop, 1000 / 30); //drawing at 30fps
            }
        })();
    });

    /*  video is currently playing from the createBackGroundCanvas function
        pause the video because the play event listener will not catch it if it is 
        already playing, then restart play to trigger event
    */
    backVideo.pause();
    backVideo.play();
}

/*  This methos is called from NewProjectDisplay.cshtml and accepts 3 arguments
    1. The ProjectVideoId which is passed in as a string including the element ID
    2. The file path for the referenced Video
    3. The thumbnail file path
    Method creates a video element and a canvas element to draw to for Overlay videos
*/
function createCanvas(id, filepath, thumbnail) {

    var video = document.createElement("video");
    video.src = filepath;
    video.width = 250;
    video.height = 150;
    video.loop = true;

    // Drag and drop adapted from W3Schools example
    var canvasElement = document.createElement("canvas");
    canvasElement.width = video.width;
    canvasElement.height = video.height;
    canvasElement.id = "c-" + id; // up to 3 canvas elements made, give custom ID
    canvasElement.draggable = true; // set this property to true in order to drag & drop
    canvasElement.ondragstart = drag; // set to call function drag once event starts


    var context = canvasElement.getContext("2d");
    // each canvas is appended to a div with a custom ID
    document.getElementById(id).appendChild(canvasElement); 

    // listen to play event for video element in order to draw to canvas
    video.addEventListener("play", function () {
        var $this = this;// cache
        (function loop() {
            if (!this.paused && !this.ended) {
                context.drawImage(video, 0, 0, canvasElement.width, canvasElement.height);
                setTimeout(loop, 1000 / 30); //drawing at 30fps
            }
        })();
    });

    // trigger play event listener
    video.play();
}


function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    var rect = ev.target.getBoundingClientRect();
    xPos = ev.clientX - rect.left - window.scrollX;
    yPos = ev.clientY - rect.top - window.scrollY;
    ev.dataTransfer.setData("text", ev.target.id);
}


function drop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData("text");
    var canvas = document.getElementById(data);

    //sconsole.log("x : ", ev.clientX, "y : ", ev.clientY);
    ev.target.parentElement.appendChild(canvas);
    var div = document.getElementById("background-video");

    var mouseX = ev.clientX - div.getBoundingClientRect().left - window.scrollX;
    var mouseY = ev.clientY - div.getBoundingClientRect().top - window.scrollY;


    canvas.style.position = "absolute";
    canvas.style.left = (mouseX - xPos) + "px";
    canvas.style.top = (mouseY - yPos) + "px";

    canvasX = Math.floor(mouseX - xPos);
    canvasY = Math.floor(mouseY - yPos);

    var ProjectId = canvas.id.replace("c-canvas-", "");

    var foundObjectinArray = false;

    for (var i = 0; i < ProjectObject.ProjectClass.length; i++) 
    {

        var currenObject = ProjectObject.ProjectClass[i];
        if (currenObject.ProjectVideosId == ProjectId) {
            currenObject.XPosition = canvasX;
            currenObject.YPosition = canvasY;
            foundObjectinArray = true;
        }        

    }

    if (foundObjectinArray == false) 
    {
        var projectVideo = {
            XPosition: canvasX,
            YPosition: canvasY,
            ProjectVideosId: ProjectId
        };

        ProjectObject.ProjectClass.push(projectVideo);
    }
}

$(document).ready(function () {
    $("#SaveButton").click(function () {
        var titleInputFieled = document.getElementById("ProjectTitle").value;
        ProjectObject.Title = titleInputFieled;

        $.ajax({
            url: "/Projects/SaveProject",
            type: 'post',
            dataType: 'json',
            data: ProjectObject,
            success: function (result) {
                console.log("I'm awesome", result);
                window.location.href = "/"
            }
        });
    });
});
                