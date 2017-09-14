var xPos; 
var yPos; 
var canvasX;
var canvasY;
var poop;
var ProjectObject = {};
ProjectObject.ProjectClass = [];

var backVideo;

function createBackGroundCanvas(filepath)
{
    backVideo = document.createElement("video");
    // add event listener Before setting the source, you want to listen to the file that is being loaded in , inorder to use MetaData function to retrieve it's  original width and height
    // developer.mozilla.org/en-US/docs/Web/Events/loadedmetadata
    backVideo.addEventListener("loadedmetadata", OnMetaData);
    backVideo.src = filepath;
    
    // set initial width and height and play() to trigger loadedmetadata event
    backVideo.width = 640;
    backVideo.height = 360;
    backVideo.loop = true;
    //backVideo.style.display = "none";
    //backVideo.id = "background-video";
    backVideo.play();
    //console.log("hey", backVideo.width);

    //document.getElementById("vid-1").appendChild(backVideo);
}

function OnMetaData(ev)
{
    poop = document.getElementById("vid-1");
    // width of parent container
    var poopWidth = poop.getBoundingClientRect().width;
    // original Width and height of loaded video retrieved from metadata
    var videoWidth = ev.target.videoWidth;
    var videoHeight = ev.target.videoHeight;
    // divide the parent container width by the video origibal width to get the percentage of width for scaling
    var AlteredOriginalWidthInPercent = poopWidth / videoWidth;
    // take the event object that was passed in and it's target which is the video element created and set it's width to the width of the parent conatiner'
    ev.target.width = poopWidth;
    // to get the proper aspect ratio, take the ev.target and set it's height to the metadataHeiht * the Altered % and set the height'
    ev.target.height = videoHeight * AlteredOriginalWidthInPercent;

    console.log("newWidth" , poopWidth);

    var backCanvas = document.createElement("canvas");
    backCanvas.id = "background-video";
    backCanvas.width = ev.target.width;
    backCanvas.height = ev.target.height;

    var context = backCanvas.getContext("2d");

    poop.appendChild(backCanvas);

    backVideo.addEventListener("play", function () {
        var $this = this;// cache
        (function loop() {
            if (!this.paused && !this.ended) {
                context.drawImage(backVideo, 0, 0, backCanvas.width, backCanvas.height);
                setTimeout(loop, 1000 / 30); //drawing at 30fps
            }
        })();
    });
    backVideo.pause();
    backVideo.play();
}

function createCanvas(id, filepath, thumbnail) {

    var video = document.createElement("video");
    video.src = filepath;
    video.width = 250;
    video.height = 150;
    video.loop = true;

    var canvasElement = document.createElement("canvas");
    canvasElement.width = video.width;
    canvasElement.height = video.height;
    canvasElement.id = "c-" + id;
    canvasElement.draggable = true;
    canvasElement.ondragstart = drag;


    var context = canvasElement.getContext("2d");
    document.getElementById(id).appendChild(canvasElement);

    video.addEventListener("play", function () {
        var $this = this;// cache
        (function loop() {
            if (!this.paused && !this.ended) {
                context.drawImage(video, 0, 0, canvasElement.width, canvasElement.height);
                setTimeout(loop, 1000 / 30); //drawing at 30fps
            }
        })();
    });
   
    video.play();
}


function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    var rect = ev.target.getBoundingClientRect();
    xPos = ev.clientX - rect.left - window.scrollX;
    yPos = ev.clientY - rect.top - window.scrollY;
    //console.log("canvas mouseX: " + xPos + "canvas mouseY: " + yPos);
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

    //console.log("mouseX: " + mouseX + ", mouseY: " + mouseY);

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
                