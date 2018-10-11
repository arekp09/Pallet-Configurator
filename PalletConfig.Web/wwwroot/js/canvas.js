document.addEventListener('DOMContentLoaded', function (event) {
    window.requestAnimationFrame = (function () {
        return window.requestAnimationFrame;
    })();

    // Input data
    var inputPalletSize = new Coordinates(100, 15, 150);
    var inputBoxSize = new Coordinates(19, 10, 14);
    var inputRowsPerLayer = 5;
    var inputColumnsPerLayer = 10;
    var inputLayersQuantity = 7;

    // Helpers
    var inputSizeMax = Math.max(inputPalletSize.X, inputPalletSize.Y, inputPalletSize.Z)
    var camera, scene, renderer, controls;
    var meshTop; //parent of all objects
    var position = new Coordinates(0, 0, 0);
    var palletSize = sizeScalling(inputPalletSize);
    var boxSize = sizeScalling(inputBoxSize);
    var zeroPosition = new Coordinates(
        position.X - (palletSize.X * 0.5) + (boxSize.X * 0.5),
        position.Y + (palletSize.Y * 0.025) + (boxSize.Y * 0.5),
        position.Z - (palletSize.Z * 0.5) + (boxSize.Z * 0.5)
    );

    function init() {
        var canvas = document.getElementById('canvas');
        var canvasWidth = canvas.getAttribute('width');
        var canvasHeight = canvas.getAttribute('height');

        // Set camera
        camera = new THREE.PerspectiveCamera(70, canvasWidth / canvasHeight, 0.01, 100);
        camera.position.z = 2;
        camera.position.y = 0.5;


        // Set scene
        scene = new THREE.Scene();

        // Set lights
        light1 = new THREE.PointLight(0x404040, 10, 100);
        light1.position.set(2, 1, 1);
        scene.add(light1);

        light2 = new THREE.PointLight(0x404040, 5, 100);
        light2.position.set(-2, -2, 2);
        scene.add(light2);

        light3 = new THREE.AmbientLight(0x404040, 2);
        scene.add(light3);

        // Create objects
        createPallet();
        createAllBoxes(inputRowsPerLayer, inputColumnsPerLayer, zeroPosition, boxSize, inputLayersQuantity);

        // Controls
        controls = new THREE.OrbitControls(camera);
        controls.addEventListener('change', renderer);
        controls.target.set(0, 0.5, 0);


        // Rendering
        renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
        renderer.setSize(canvasWidth, canvasHeight);
        canvas.appendChild(renderer.domElement);    
    }

    function animate() {
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
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
        var material = new THREE.MeshLambertMaterial({map: THREE.ImageUtils.loadTexture('images/palletTexture.jpg')});

        // Create top of pallet
        var geometryTop = new THREE.BoxGeometry(palletSize.X, (palletSize.Y * 0.05), palletSize.Z);
        meshTop = new THREE.Mesh(geometryTop, material)
        scene.add(meshTop);
        meshTop.position.set(position.X, position.Y, position.Z);        
        
        // Create bottom of pallet
        var geometryBottom = new THREE.BoxGeometry(palletSize.X * 0.1, palletSize.Y * 0.05, palletSize.Z);
        for (var i = 0; i < 3; i++) {
            var meshBottom = new THREE.Mesh(geometryBottom, material);
            scene.add(meshBottom);
            meshBottom.position.set(position.X + (palletSize.X * 0.45) * (i-1), position.Y - (palletSize.Y * 0.95), position.Z);
            meshTop.add(meshBottom);
        }
        
        // Create middle bricks
        var geometryBrick = new THREE.BoxGeometry(palletSize.X * 0.1, palletSize.Y * 0.9, palletSize.Z * 0.1);
        var brickPosX = position.X - (palletSize.X * 0.45);
        var brickPosY = position.Y - (palletSize.Y * 0.475);
        var brickPosZ = position.Z - (palletSize.Z * 0.45);

        for (var i = 0; i < 3; i++) {
            for (var j = 0; j < 3; j++) {
                var meshBrick = new THREE.Mesh(geometryBrick, material);
                scene.add(meshBrick);
                meshTop.add(meshBrick);
                meshBrick.position.set(brickPosX, brickPosY, brickPosZ);
                brickPosX += palletSize.X * 0.45;
            }
            brickPosX = position.X - (palletSize.X * 0.45);
            brickPosZ += (palletSize.Z * 0.45);;
        }    
    }

    function sizeScalling(_inputSize) {
        _inputSize.X = (_inputSize.X / inputSizeMax) * 2;
        _inputSize.Y = (_inputSize.Y / inputSizeMax) * 2;
        _inputSize.Z = (_inputSize.Z / inputSizeMax) * 2;
        return _inputSize;
    }

    function createAllBoxes(_numberRows, _numberColumns, _position, _boxSize, _layersQuantity) {
        var posX = _position.X;
        var posY = _position.Y;
        var posZ = _position.Z;
        
        for (var i = 0; i < _layersQuantity; i++) {
            generateLayer(_numberRows, _numberColumns, new Coordinates(X = posX, Y = posY, Z = posZ), _boxSize);
            posX = _position.X;
            posY += _boxSize.Y;
            posZ = _position.Z;
        }
    }

    function generateLayer(_numberRows, _numberColumns, _position, _boxSize) {
        for (var i = 0; i < _numberRows; i++) {
            for (var j = 0; j < _numberColumns; j++) {
                generateBox(_boxSize, _position.X + (_boxSize.X * i), _position.Y, _position.Z + (_boxSize.Z * j));
            }
        }
    }

    function generateBox(_boxSize, posX, posY, posZ) {
        var geometryBox = new THREE.BoxGeometry(_boxSize.X, _boxSize.Y, _boxSize.Z);
        var materialBox = new THREE.MeshLambertMaterial({ map: THREE.ImageUtils.loadTexture('images/boxTexture.jpg'), color: 0xb38600 });
        var meshBox = new THREE.Mesh(geometryBox, materialBox);
        scene.add(meshBox);
        meshTop.add(meshBox);
        meshBox.position.set(posX, posY, posZ);
    }

    init();
    animate();
});