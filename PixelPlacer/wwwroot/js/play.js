var VideoArray = [];
var BackGroundCanvasArray = [];
var CanvasArray = [];
var videoTypeId;
var backGround;
var canvas;
//var videoOverLayElement;

function CreateBackGround(backProjectVidoId, backFilePath, videoType)
{
    var counter = 1;
    console.log("background video :", backFilePath, backProjectVidoId, videoType);
    videoTypeId = videoType;

    backGround = document.createElement("video");
    backGround.addEventListener("loadedmetadata", BackGroundMetaData);

    backGround.src = backFilePath;
    backGround.id = "v-" + counter;
    backGround.width = 640;
    backGround.height = 360;
    backGround.loop = true;

    VideoArray.push(backGround);
}

function BackGroundMetaData(ev)
{
    var counter = 1;
    var parentContainer = document.getElementById("parentContainer");
    var parentContainerWidth = parentContainer.getBoundingClientRect().width;
    var videoOriginalWidth = ev.target.videoWidth;
    var videoOriginalHeight = ev.target.videoHeight;
    var AlteredOriginalWidthInPercent = parentContainerWidth / videoOriginalWidth;

    console.log("cantainer width:", parentContainerWidth);
    ev.target.width = parentContainerWidth;
    ev.target.height = videoOriginalHeight * AlteredOriginalWidthInPercent;

    if (videoTypeId == 2) {
        var backCanvas = document.createElement("canvas");
        backCanvas.width = ev.target.width;
        backCanvas.height = ev.target.height;
        backCanvas.id = "c-" + counter;
        CanvasArray.push(backCanvas);
        backCanvas.addEventListener("click", ClickToPlay);
        //backCanvas.addEventListener("mouseover", MouseOver);

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
        backCanvas.width = ev.target.width;
        backCanvas.height = ev.target.height;
        CanvasArray.push(backCanvas);
        backCanvas.addEventListener("click", ClickToPlay);
        //backCanvas.addEventListener("mouseover", MouseOver);


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

function CreateOverLay(ProjVideoId, filepath)
{
    counter = 2;
    var parentContainer = document.getElementById("parentContainer");
    var videoOverLayElement = document.createElement("video");
    videoOverLayElement.id = "o-" + counter;
    videoOverLayElement.src = filepath;
    videoOverLayElement.width = 250;
    videoOverLayElement.height = 150;
    videoOverLayElement.loop = true;
    VideoArray.push(videoOverLayElement);


    canvas = document.createElement("canvas");
    canvas.width = videoOverLayElement.width;
    canvas.height = videoOverLayElement.height;
    canvas.id = "c-" + counter;
    canvas.style.position = "absolute";
    canvas.addEventListener("click", ClickToPlay);
    //canvas.addEventListener("mouseover", MouseOver);

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

    seriously.go();
    videoOverLayElement.play();
    counter++
}


$(document).ready(function () {
    $("#PlayButton").click(PlayAll);
    $("#StopButton").click(StopAll);
});


// Function to play all videos and remove eventlisteners for mouseover, mouseout, and click on individual canvas
function PlayAll(ev)
{
    for (var i = 0; i < VideoArray.length; i++)
    {
        VideoArray[i].play();
        CanvasArray[i].removeEventListener("mouseover", MouseOver);
        //CanvasArray[i].removeEventListener("mouseout", MouseOut);
        //CanvasArray[i].removeEventListener("click", ClickToPlay);
    }
}

function StopAll(ev)
{
    for (var i = 0; i < VideoArray.length; i++)
    {
        VideoArray[i].pause();
        VideoArray[i].currentTime = 0;
        CanvasArray[i].addEventListener("mouseover", MouseOver);
        //CanvasArray[i].addEventListener("mouseout", MouseOut);
        CanvasArray[i].addEventListener("click", ClickToPlay);
    }
}

function ClickToPlay(ev)
{
    console.log("Who is target:", ev.target.id);
    
    if (ev.target.id == "c-1") {
        if (VideoArray[0].paused) {
            VideoArray[0].play();
            ev.target.removeEventListener("mouseover", MouseOver);
            //ev.target.removeEventListener("mouseout", MouseOut);
        } else {
            VideoArray[0].pause();
            //ev.target.addEventListener("mouseover", MouseOver);
            //ev.target.addEventListener("mouseout", MouseOut);
        }
    } else {
        var canvasId = ev.target.id.slice(2, 3);
        for (var i = 0; i < CanvasArray.length; i++)
        {
            if (canvasId == VideoArray[i].id.slice(2, 3))
            {
                console.log("tell me I'm awesome");
                if (VideoArray[i].paused)
                {
                    VideoArray[i].play();
                    ev.target.removeEventListener("mouseover", MouseOver);
                    //ev.target.removeEventListener("mouseout", MouseOut);
                } else {
                    VideoArray[i].pause();
                    //ev.target.addEventListener("mouseover", MouseOver);
                    //ev.target.addEventListener("mouseout", MouseOut);
                }          
            }
        }
    }    
}

function MouseOver(ev)
{
    //console.log("mouseover:", ev.target);
    var targetId = ev.target.id.slice(2, 3);
        //console.log("what might you be?", VideoArray[targetId - 1].id, ev.target.id);
    var videoId = VideoArray[targetId - 1]
    if (videoId.paused) {
        videoId.play();
    } else {
        videoId.pause();
    }
}











