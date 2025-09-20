// Tetris Canvas Animation for Razor Pages background
(function () {
    const canvas = document.getElementById("tetrisCanvas");
    if (!canvas) return;
    const ctx = canvas.getContext("2d");

    function resizeCanvas() {
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;
    }
    window.addEventListener('resize', resizeCanvas);
    resizeCanvas();

    class Shape {
        constructor(x, y, type, color) {
            this.x = x;
            this.y = y;
            this.type = type;
            this.color = color;
            this.speed = Math.random() * 2 + 1;
        }
        update() {
            this.y += this.speed;
            if (this.y > canvas.height + 40) {
                this.y = -40;
                this.x = Math.random() * (canvas.width - 40);
                this.type = Math.random() < 0.5 ? 'cube' : 'circle';
            }
        }
        draw(ctx) {
            ctx.fillStyle = this.color;
            if (this.type === 'cube') {
                ctx.fillRect(this.x, this.y, 40, 40);
            } else if (this.type === 'circle') {
                ctx.beginPath();
                ctx.arc(this.x + 20, this.y + 20, 20, 0, Math.PI * 2);
                ctx.fill();
            }
        }
    }

    const shapes = [];
    const totalPerColor = 24;
    for (let i = 0; i < totalPerColor; i++) {
        const x = Math.random() * (canvas.width - 40);
        const y = Math.random() * canvas.height;
        shapes.push(new Shape(x, y, Math.random() < 0.5 ? 'cube' : 'circle', 'red'));
        shapes.push(new Shape(x, y, Math.random() < 0.5 ? 'cube' : 'circle', 'blue'));
    }

    function loop() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        shapes.forEach(shape => {
            shape.update();
            shape.draw(ctx);
        });
        requestAnimationFrame(loop);
    }
    loop();
})();
