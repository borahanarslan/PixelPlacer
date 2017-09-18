var VideoArray = [];
var BackGroundCanvasArray = [];
var CanvasArray = [];
var VideoMouseOverArray = [];
var CanvasMouseOverArray = [];
var OverlaySizePercentage;
var videoTypeId;
var backGround;
var canvas;
var Xposition;
var Yposition;
var Width;
var Height;
var Rotation;
var backgroundVidHeight;
var backgroundVidWidth;
var parentContainerWidth;
var globalCounter = 2;
var SizeInPercent;

function CreateBackGround(backProjectVidoId, backFilePath, videoType, w, h)
{
    var counter = 1;
    var parentContainer = document.getElementById("parentContainer");
    var parentContainerWidth = parentContainer.getBoundingClientRect().width;
    SizeInPercent = parentContainerWidth / w;

    backgroundVidWidth = w;
    backgroundVidHeight = h;

    var navBarHide = document.getElementById("Hide-Nav-on-PlayBack");
    document.body.style.padding = "0";
    navBarHide.style.display = "none";

    var counter = 1;
    videoTypeId = videoType;

    backGround = document.createElement("video");

    backGround.src = backFilePath;
    backGround.id = "v-" + counter;
    backGround.width = backgroundVidWidth * SizeInPercent;
    backGround.height = backgroundVidHeight * SizeInPercent;
    backGround.loop = true;

    VideoArray.push(backGround);

    if (videoTypeId == 2) {
        var backCanvas = document.createElement("canvas");
        backCanvas.width = backGround.width;
        backCanvas.height = backGround.height;
        backCanvas.id = "c-" + counter;
        backCanvas.style.verticalAlign = "bottom";
     


        var seriously = new Seriously();
        var src = seriously.source(backGround);
        var target = seriously.target(backCanvas);
        var chroma = seriously.effect("chroma");
        chroma.source = src;

        var resize = seriously.transform("reformat");
        resize.width = backCanvas.width;
        resize.height = backCanvas.height;
        resize.mode = 'distort';
        resize.source = chroma;
        target.source = resize;

        var r = 98 / 255;
        var g = 175 / 255;
        var b = 116 / 255;
        chroma.screen = [r, g, b, 1];

        parentContainer.appendChild(backCanvas);
        seriously.go();
        backGround.play();
    } else {
        var backCanvas = document.createElement("canvas");
        backCanvas.id = "c-" + counter;
        backCanvas.width = backGround.width;
        backCanvas.height = backGround.height;
        CanvasArray.push(backCanvas);

        var context = backCanvas.getContext("2d");
        parentContainer.appendChild(backCanvas);

        backGround.addEventListener("play", function () {
            var $this = this;
            (function loop() {
                if (!this.paused && !this.ended) {
                    context.drawImage(backGround, 0, 0, backCanvas.width, backCanvas.height);
                    setTimeout(loop, 1000 / 30);
                }
            })();
        });
        backGround.play();
    }
}


function CreateOverLay(ProjVideoId, filepath, x, y, w, h, r)
{
    Xposition = x;
    Yposition = y;
    Width = w;
    Height = h;
    Rotation = r;
    globalCounter++; 

    var parentContainer = document.getElementById("parentContainer");
    var videoOverLayElement = document.createElement("video");
    videoOverLayElement.id = "o-" + globalCounter;
    videoOverLayElement.src = filepath;
    videoOverLayElement.width = Width * SizeInPercent;
    videoOverLayElement.height = Height * SizeInPercent;
    videoOverLayElement.loop = true;


    VideoArray.push(videoOverLayElement);
    VideoMouseOverArray.push(videoOverLayElement);

    canvas = document.createElement("canvas");
    canvas.width = videoOverLayElement.width ;
    canvas.height = videoOverLayElement.height;
    canvas.id = "c-" + globalCounter;
    canvas.style.position = "absolute";
    canvas.style.left = Xposition + "px";
    canvas.style.top = Yposition + "px";
    canvas.style.rotation = Rotation;

    var seriously = new Seriously();
    var src = seriously.source(videoOverLayElement);
    var target = seriously.target(canvas);
    var chroma = seriously.effect("chroma");
    chroma.source = src;

    var resize = seriously.transform("reformat");
    resize.width = 250;
    resize.height = 150;
    resize.mode = 'distort';
    resize.source = chroma;
    target.source = resize;

    var r = 98 / 255;
    var g = 175 / 255;
    var b = 116 / 255;
    chroma.screen = [r, g, b, 1];

    parentContainer.appendChild(canvas);
    CanvasArray.push(canvas);
    CanvasMouseOverArray.push(canvas);

    seriously.go();
    videoOverLayElement.play();
    
}


$(document).ready(function () {
    $("#PlayButton").click(PlayAll);
    $("#StopButton").click(StopAll);
    $("#InteractiveButton").click(InteractivePlay);
});


 //Function to play all videos and remove eventlisteners for mouseover, mouseout, and click on individual canvas
function PlayAll(ev)
{
    for (var i = 0; i < VideoArray.length; i++)
    {
        if (VideoArray[i].paused) {
            VideoArray[i].play();
            CanvasArray[i].removeEventListener("mouseover", BackGroundMouseOver);
            CanvasArray[i].removeEventListener("mouseout", MouseOut);
            CanvasArray[i].removeEventListener("click", ClickToPlay);
        } else {
            VideoArray[i].pause();
        }
    }
}


function StopAll(ev)
{
    for (var i = 0; i < VideoArray.length; i++)
    {
        VideoArray[i].currentTime = 0;
        VideoArray[i].play();
        CanvasArray[i].removeEventListener("mouseover", BackGroundMouseOver);
        CanvasArray[i].removeEventListener("mouseout", MouseOut);
        CanvasArray[i].removeEventListener("click", ClickToPlay);
    }
}

function InteractivePlay(ev)
{
    for (var i = 0; i < VideoArray.length; i++)
    {
        VideoArray[i].pause();
        VideoArray[i].currentTime = 0;
        if (CanvasArray[i].id == "c-1")
        {
            CanvasArray[i].addEventListener("mouseover", BackGroundMouseOver);
        }
        if (CanvasArray[i].id != "c-1")
        {
            for (var b = 0; b < CanvasMouseOverArray.length; b++)
            {
                CanvasMouseOverArray[b].addEventListener("mouseover", OverlayMouseOver);
                CanvasMouseOverArray[b].addEventListener("click", ClickToPlay);
            }
        }
    }
}

function ClickToPlay(ev)
{    
    if (ev.target.id == "c-1")
    {
        if (VideoArray[0].paused)
        {
            VideoArray[0].play();
        } else
        {
            VideoArray[0].pause();    
        }
    } else
    {
        var canvasId = ev.target.id.slice(2, 3);
        for (var i = 0; i < CanvasArray.length; i++)
        {
            if (canvasId == VideoArray[i].id.slice(2, 3))
            {
                console.log("tell me I'm awesome");
                if (VideoArray[i].paused)
                {
                    VideoArray[i].play();
                } else {
                    VideoArray[i].pause();
                }          
            }
        }
    }    
}


function OverlayMouseOver(ev)
{
    for (var i = 0; i < VideoMouseOverArray.length; i++) 
    {
        console.log("array", VideoMouseOverArray);
        if (ev.target.id.replace("c-", "") == VideoMouseOverArray[i].id.replace("o-", "")) 
        {
            ev.target.addEventListener("mouseout", MouseOut);
            VideoMouseOverArray[i].play();
        }
        
    }
}


function BackGroundMouseOver(ev)
{
    var backgroundVideoTarget = VideoArray[0];

    if (ev.target.id == "c-1") {
        backgroundVideoTarget.play();
    } 

    if (ev.target.id != "c-1")
    {
        for (var i = 0; i < VideoMouseOverArray.length; i++)
        {
            var canvasTargetId = ev.target.id.replace("c-", "");
            var videoTargetId = VideoMouseOverArray[i].id.replace("o-", "");
            if (canvasTargetId == videoTargetId) {
                ev.target.addEventListener("mouseout", MouseOut);
                VideoMouseOverArray[i].play();
            } else if (canvasTargetId != VideoMouseOverArray[i].id.replace("o-", "")) 
            {
                CanvasMouseOverArray[i].removeEventListener("mouseover", MouseOver);
                VideoMouseOverArray[i].pause();
            }
        }
    } 
}


function MouseOut(ev)
{
    var targetId = ev.target.id.replace("c-", "");
    for (var i = 0; i < VideoMouseOverArray.length; i++)
    {
        var videoId = VideoMouseOverArray[i].id.replace("o-", "");
        if (targetId == videoId)
        {
            VideoMouseOverArray[i].pause();
        }
    }
}








