document.addEventListener('DOMContentLoaded', function (event) {
    window.requestAnimationFrame = (function () {
        return window.requestAnimationFrame;
    })();

    var camera, scene, renderer;
    var geometry, material, mesh;
    var isAnimationOn = true;

    function init() {
        var canvas = document.getElementById('canvas');
        var canvasWidth = canvas.getAttribute('width');
        var canvasHeight = canvas.getAttribute('height');


        camera = new THREE.PerspectiveCamera(70, canvasWidth / canvasHeight, 0.01, 100);
        camera.position.z = 2;

        scene = new THREE.Scene();

        //geometry = new THREE.BoxGeometry(0.2, 0.2, 0.5);
        //var edges = new THREE.EdgesGeometry(geometry);
        //var line = new THREE.LineSegments(edges, new THREE.LineBasicMaterial({ color: 0xff0000 }));
        //var line2 = new THREE.LineSegments(edges, new THREE.LineBasicMaterial({ color: 0xff0000 }));
        //scene.add(line);
        //scene.add(line2);

        //material = new THREE.MeshPhongMaterial({ color: 0x00ff00 });

        light1 = new THREE.PointLight(0x404040, 10, 100);
        light1.position.set(1, 1, 1);
        scene.add(light1);

        light2 = new THREE.PointLight(0x404040, 5, 100);
        light2.position.set(-1, -1, 0);
        scene.add(light2);

        createPallet();

        //mesh = new THREE.Mesh(geometry, material);
        //scene.add(mesh);
        //mesh.position.set(0, 0, 0);
        //line.position.set(0, 0, 0);
        //mesh.add(line);

        //var mesh2 = new THREE.Mesh(geometry, material);
        //scene.add(mesh2);
        //mesh2.position.set(0.2, 0, 0);
        //line2.position.set(0.2, 0, 0);
        //mesh.add(line2);
        //mesh.add(mesh2);

        

        renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
        renderer.setSize(canvasWidth, canvasHeight);
        canvas.appendChild(renderer.domElement);
        //mesh.rotation.x = 0.3;
    }

    function animate() {
        if (isAnimationOn) {
            requestAnimationFrame(animate);
            //mesh.rotation.x += 0.01;
            //mesh.rotation.y += 0.005;
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
        var position = new Coordinates(0, 0, 0);
        var palletSize = new Coordinates(2, 0.5, 1);

        //Create top flat layer for pallet
        createPalletFlat(palletSize, position);

        //Create middle bricks for pallet


        //Create bottom flat layer for pallet
        
    }

    function createPalletFlat(_palletSize, _position) {
        geometryFlat = new THREE.BoxGeometry(_palletSize.X, (_palletSize.Y*0.05), _palletSize.Z);
        var edgesFlat = new THREE.EdgesGeometry(geometryFlat);
        var lineEdgesFlat = new THREE.LineSegments(edgesFlat, new THREE.LineBasicMaterial({ color: 0x000000 }));
        var materialFlat = new THREE.MeshBasicMaterial({ color: 0xb38600 });
        var meshFlat = new THREE.Mesh(geometryFlat, materialFlat)
        scene.add(lineEdgesFlat);
        scene.add(meshFlat);
        meshFlat.add(lineEdgesFlat);
        meshFlat.position.set(_position.X, _position.Y, _position.Z);
        meshFlat.rotation.x = 0.2;
        meshFlat.rotation.y = 0.2;
    }

    function createPalletBrick() {

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
