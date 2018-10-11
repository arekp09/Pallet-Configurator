document.addEventListener('DOMContentLoaded', function (event) {
    window.requestAnimationFrame = (function () {
        return window.requestAnimationFrame;
    })();

    var camera, scene, renderer;
    var meshTop;
    var position = new Coordinates(0, 0, 0);
    var palletSize = new Coordinates(1, 0.2, 1.5);
    var isAnimationOn = true;

    function init() {
        var canvas = document.getElementById('canvas');
        var canvasWidth = canvas.getAttribute('width');
        var canvasHeight = canvas.getAttribute('height');

        camera = new THREE.PerspectiveCamera(70, canvasWidth / canvasHeight, 0.01, 100);
        camera.position.z = 2;
        camera.position.y = 0.5;
        //camera.position.x = -1;

        scene = new THREE.Scene();

        light1 = new THREE.PointLight(0x404040, 10, 100);
        light1.position.set(1, 1, 1);
        scene.add(light1);

        light2 = new THREE.PointLight(0x404040, 5, 100);
        light2.position.set(-1, -1, 0);
        scene.add(light2);

        createPallet();

        renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
        renderer.setSize(canvasWidth, canvasHeight);
        canvas.appendChild(renderer.domElement);       
    }

    function animate() {
        if (isAnimationOn) {
            requestAnimationFrame(animate);
            //meshTop.rotation.x += 0.01;
            meshTop.rotation.y += 0.005;
            renderer.render(scene, camera);
        }
    }

    function Coordinates(X, Y, Z) {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
        this.area = function () {
            return this.X * this.Y * this.Z;
        }
    }

    function createPallet() {
        var material = new THREE.MeshPhongMaterial({ color: 0xb38600 });

        // Create top of pallet
        var geometryTop = new THREE.BoxGeometry(palletSize.X, (palletSize.Y * 0.05), palletSize.Z);
        var edgesTop = new THREE.EdgesGeometry(geometryTop);
        var lineEdgesTop = new THREE.LineSegments(edgesTop, new THREE.LineBasicMaterial({ color: 0x000000 }));
        meshTop = new THREE.Mesh(geometryTop, material)
        scene.add(lineEdgesTop);
        scene.add(meshTop);
        meshTop.add(lineEdgesTop);
        meshTop.position.set(position.X, position.Y, position.Z);        
        
        // Create bottom of pallet
        var lineEdgesBottom = new THREE.LineSegments(edgesTop, new THREE.LineBasicMaterial({ color: 0x000000 }));
        var meshBottom = new THREE.Mesh(geometryTop, material);
        scene.add(lineEdgesBottom);
        scene.add(meshBottom);
        meshBottom.add(lineEdgesBottom);
        meshBottom.position.set(position.X, position.Y - (palletSize.Y * 0.95), position.Z);
        meshTop.add(meshBottom);
        
        // Create middle bricks
        var geometryBrick = new THREE.BoxGeometry(palletSize.X * 0.1, (palletSize.Y * 0.9), palletSize.Z);
        var edgesBrick = new THREE.EdgesGeometry(geometryBrick);
        var lineEdgesBrick1 = new THREE.LineSegments(edgesBrick, new THREE.LineBasicMaterial({ color: 0x000000 }));
        var lineEdgesBrick2 = new THREE.LineSegments(edgesBrick, new THREE.LineBasicMaterial({ color: 0x000000 }));
        var lineEdgesBrick3 = new THREE.LineSegments(edgesBrick, new THREE.LineBasicMaterial({ color: 0x000000 }));
        var meshBrick1 = new THREE.Mesh(geometryBrick, material);
        var meshBrick2 = new THREE.Mesh(geometryBrick, material);
        var meshBrick3 = new THREE.Mesh(geometryBrick, material);
        scene.add(lineEdgesBrick1);
        scene.add(lineEdgesBrick2);
        scene.add(lineEdgesBrick3);
        scene.add(meshBrick1);
        scene.add(meshBrick2);
        scene.add(meshBrick3);
        meshBrick1.add(lineEdgesBrick1);
        meshBrick2.add(lineEdgesBrick2);
        meshBrick3.add(lineEdgesBrick3);
        meshTop.add(meshBrick1);
        meshTop.add(meshBrick2);
        meshTop.add(meshBrick3);
        meshBrick1.position.set(position.X - (palletSize.X * 0.45), position.Y - (palletSize.Y * 0.5), position.Z);
        meshBrick2.position.set(position.X, position.Y - (palletSize.Y * 0.5), position.Z);
        meshBrick3.position.set(position.X + (palletSize.X * 0.45), position.Y - (palletSize.Y * 0.5), position.Z);        
    }

    init();
    animate();

    document.onclick = function () {
        if (isAnimationOn) {
            isAnimationOn = false;
        }
        else {
            isAnimationOn = true;
        }
        animate();
    }
});