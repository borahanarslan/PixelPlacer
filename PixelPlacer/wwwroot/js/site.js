var xPos; 
var yPos; 
var canvasX;
var canvasY;
var ProjectObject = {};
ProjectObject.ProjectClass = [];


counter = 0;
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


    document.getElementById(id).appendChild(canvasElement);

    var context = canvasElement.getContext("2d");


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


$("#SaveButton").click(function () {
    var titleInputFieled = document.getElementById("ProjectTitle").value;
    ProjectObject.Title = titleInputFieled;
    console.log("Did my object make", ProjectObject);

        $.ajax({
            url: "/Projects/SaveProject",
            type: 'post',
            dataType: 'application/json',
            data: ProjectObject,

            success: function (result) {
                if (response.result == 'Redirect') {
                    window.location.href = "http://localhost:61006/Home/Index"
                }
            }
        });       
});                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
                //window.location.href = "http://localhost:61006/Home/Index"
                //console.log("success message: ", result);