var xPos; // store mouse X position relative to container that is dragging
var yPos; // store mouse Y position relative to container that is dragging
var canvasX; // store X position of container that is dragging
var canvasY; // store Y position of container that is dragging
var backgroundParent; // parent container background canvas element will be appended to
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
    backgroundParent = document.getElementById("vid-1");
    
    /*  getBoundingClientRect retrieves the parent container
        has a property of width, the width will be used to set the width of the canvas
    */
    var backgroundParentWidth = backgroundParent.getBoundingClientRect().width;

    // original Width and height of loaded video retrieved from metadata
    var videoWidth = ev.target.videoWidth;
    var videoHeight = ev.target.videoHeight;

    /*  divide the parent container width by the video original width to get the
        percentage of width for scaling
    */
    var AlteredOriginalWidthInPercent = backgroundParentWidth / videoWidth;

    /*  take the event object that was passed in and it's target 
        which is the video element created and set it's width to the width 
        of the parent conatiner
    */
    ev.target.width = backgroundParentWidth;


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

// set receiving container of drop to allow
function allowDrop(ev) {
    ev.preventDefault();
}

//event that is passed in the is canvas element that is currently targeted to drag   
function drag(ev) {
    /*  get position of targeted element container in relation to the body of the page
        the getBoundingClientRect Method has a left and top property
    */
    var rect = ev.target.getBoundingClientRect();
    /*  ev.clientX  and clientY retrieves the mouse position in relation to the page
        take that position and subtract the X position of the targeted canvas
        this gives the mouse X position in relation to the targeted container
        window has a property of scrollX and ScrollY
        These properties account for extra width and height added if the page is made
        smaller. When page shrinks it adds scroll height and/or width
        Then do the same for the Y position
    */
    xPos = ev.clientX - rect.left - window.scrollX;
    yPos = ev.clientY - rect.top - window.scrollY;
    ev.dataTransfer.setData("text", ev.target.id);// stages data to be dropped
}


function drop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData("text"); //receives data that is dropped
    var canvas = document.getElementById(data); // the element that is dropped

    /*  Element that is receiving the drop is the target and is within a 
        from the ev.target get it's parent and append the transferred data
        to the parent, this keeps the new canvas from going inside the targeted canvas
    */
    ev.target.parentElement.appendChild(canvas); 

    // event.target receiving element
    var div = document.getElementById("background-video");

    /*  to get the x and y position accurate for the dropped element
        retrieve the mouse X and Y position relative to the body on the drop process
        Then get the X and Y, or Left and Top position, of the element that is receiving
        the dropped data in relation to the page
        Subtract the mouse Drop position from the Container position and account for
        scroll width and height
    */
    var mouseX = ev.clientX - div.getBoundingClientRect().left - window.scrollX;
    var mouseY = ev.clientY - div.getBoundingClientRect().top - window.scrollY;

    // overlay should be set to absolute so it will sit on top
    canvas.style.position = "absolute"; 
    // set it's position, position of the mouse on container minus position of mouse on
    // dragged container
    canvas.style.left = (mouseX - xPos) + "px";
    canvas.style.top = (mouseY - yPos) + "px";

    // positions with multiple decimal places would not insert into DB, Floor fixed that
    canvasX = Math.floor(mouseX - xPos);
    canvasY = Math.floor(mouseY - yPos);

    // to pass the ProjectVideoId remove the string added to make a custome id
    var ProjectId = canvas.id.replace("c-canvas-", "");

    // every time canvas element is moved, a new object was made and added to an Array
    // set variable to check if object exists
    var foundObjectinArray = false;

    // To avoid duplicate objects in the array, loop through the Array length
    for (var i = 0; i < ProjectObject.ProjectClass.length; i++) 
    {
        // if the object already exists, update it's position
        var currenObject = ProjectObject.ProjectClass[i];
        if (currenObject.ProjectVideosId == ProjectId) {
            currenObject.XPosition = canvasX;
            currenObject.YPosition = canvasY;
            foundObjectinArray = true;
        }        
    }

    if (foundObjectinArray == false) 
    {
        // if object does not exist create a new object and push into Array
        var projectVideo = {
            XPosition: canvasX,
            YPosition: canvasY,
            ProjectVideosId: ProjectId
        };
        ProjectObject.ProjectClass.push(projectVideo);
    }
}

/*  Event Listener on Save Button on NewProjectDisplay.cshtml
Updates the title in the DB for a ProjectObjectOnce a title has been added
the project is no longer open
Event also saves the Canvas position of the overlay videos in the DB
*/ 
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
                