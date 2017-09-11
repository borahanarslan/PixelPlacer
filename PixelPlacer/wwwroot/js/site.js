// Write your Javascript code.

counter = 0;
function createCanvas(filepath) {

    var video = document.createElement("video");
    video.src = `${filepath}`
    video.width = 200;
    video.height = 180;

    var canvasElement = document.createElement("canvas");
    canvasElement.id = "canvas-" + counter++;
    canvasElement.width = video.width;
    canvasElement.height = video.height;

    document.getElementsByClassName("overlayVideo").appendChild(canvas);

    var context = canElement.getContext("2d");

    video.addEventListener("play", function () {
        var $this = this;// cache
        (function loop() {
            if (!this.paused && !this.ended) {
                context.drawImage(video, 0, 0, canvasElement.width, canvasElement.height);
                setTimeout(loop, 1000 / 30); //drawing at 30fps
            }
        })();
    }, 0);
}





