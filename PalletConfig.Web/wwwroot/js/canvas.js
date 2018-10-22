//document.addEventListener('DOMContentLoaded', function (event) {
//    window.requestAnimationFrame = (function () {
//        return window.requestAnimationFrame;
//    })();

// Input data
var inputPalletSize, inputBoxSize;
var inputRowsPerLayer, inputColumnsPerLayer, inputLayersQuantity;

function Coordinates(X, Y, Z) {
    this.X = X;
    this.Y = Y;
    this.Z = Z;
    this.area = function () {
        return this.X * this.Y * this.Z;
    }
}

function draw3D() {
    window.requestAnimationFrame = (function () {
        return window.requestAnimationFrame;
    })();
    
    // Helper varibles
    var materialPallet = new THREE.MeshLambertMaterial({ map: new THREE.TextureLoader().load('images/palletTexture.jpg') });
    var materialBox = new THREE.MeshLambertMaterial({ map: new THREE.TextureLoader().load('images/boxTexture.jpg'), color: 0xb38600 });
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
    var maxPosition = new Coordinates(
        position.X + (palletSize.X * 0.5) - (boxSize.X * 0.5),
        position.Y + (palletSize.Y * 0.025) + (boxSize.Y * 0.5),
        position.Z + (palletSize.Z * 0.5) - (boxSize.Z * 0.5)
    );
    var areLayersOpposite = document.getElementById('stackOpposite').checked;

    function init() {
        var canvas = document.getElementById('canvas');
        var canvasWidth = canvas.getAttribute('width');
        var canvasHeight = canvas.getAttribute('height');

        // Set camera
        camera = new THREE.PerspectiveCamera(70, canvasWidth / canvasHeight, 0.01, 100);
        camera.position.set(1, 1, 3);

        // Set scene
        scene = new THREE.Scene();

        // Set lights
        light1 = new THREE.PointLight(0x404040, 5, 100);
        light1.position.set(0, 3, 0);
        scene.add(light1);

        light2 = new THREE.PointLight(0x404040, 5, 100);
        light2.position.set(-2, -2, 2);
        scene.add(light2);

        light3 = new THREE.AmbientLight(0x404040, 2.5);
        scene.add(light3);

        // Create objects
        createPallet();
        createAllBoxes(inputRowsPerLayer, inputColumnsPerLayer, zeroPosition, boxSize, inputLayersQuantity, maxPosition, areLayersOpposite);

        // Controls
        controls = new THREE.OrbitControls(camera, canvas);
        controls.target = new THREE.Vector3(0, 0.5, 0);
        controls.update();

        // Rendering
        renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
        renderer.setSize(canvasWidth, canvasHeight);
        canvas.appendChild(renderer.domElement);    
    }

    function animate() {
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
    }

    function createPallet() {
        // Create top of pallet
        var geometryTop = new THREE.BoxGeometry(palletSize.X, (palletSize.Y * 0.05), palletSize.Z);
        meshTop = new THREE.Mesh(geometryTop, materialPallet)
        scene.add(meshTop);
        meshTop.position.set(position.X, position.Y, position.Z);        
        
        // Create bottom of pallet
        var geometryBottom = new THREE.BoxGeometry(palletSize.X * 0.1, palletSize.Y * 0.05, palletSize.Z);
        for (var i = 0; i < 3; i++) {
            var meshBottom = new THREE.Mesh(geometryBottom, materialPallet);
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
                var meshBrick = new THREE.Mesh(geometryBrick, materialPallet);
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

    function createAllBoxes(_numberRows, _numberColumns, _zeroPosition, _boxSize, _layersQuantity, _maxPosition, _areLayersOpposite) {
        var posX = _zeroPosition.X;
        var posY = _zeroPosition.Y;
        var posZ = _zeroPosition.Z;
        var changeSide = false;
        
        for (var i = 0; i < _layersQuantity; i++) {
            generateLayer(_numberRows, _numberColumns, new Coordinates(X = posX, Y = posY, Z = posZ), _boxSize, changeSide);

            // Move position up to another layer
            posY += _boxSize.Y;

            // Check if user want to stack layers opposite
            if (_areLayersOpposite == true) {
                if (changeSide == true) {
                    changeSide = false;
                }
                else {
                    changeSide = true;
                }

                // Change coordinates of starting position
                if (changeSide == true) {
                    posX = _maxPosition.X;
                    posZ = _maxPosition.Z;
                }
                else {
                    posX = _zeroPosition.X;
                    posZ = _zeroPosition.Z;
                }
            }
        }
    }

    function generateLayer(_numberRows, _numberColumns, _position, _boxSize, _changeSide) {
        for (var i = 0; i < _numberRows; i++) {
            for (var j = 0; j < _numberColumns; j++) {
                if (_changeSide == true) {
                    generateBox(_boxSize, _position.X - (_boxSize.X * i), _position.Y, _position.Z - (_boxSize.Z * j));
                }
                else {
                    generateBox(_boxSize, _position.X + (_boxSize.X * i), _position.Y, _position.Z + (_boxSize.Z * j));
                }
            }
        }
    }

    function generateBox(_boxSize, posX, posY, posZ) {
        var geometryBox = new THREE.BoxGeometry(_boxSize.X, _boxSize.Y, _boxSize.Z);
        var meshBox = new THREE.Mesh(geometryBox, materialBox);
        scene.add(meshBox);
        meshTop.add(meshBox);
        meshBox.position.set(posX, posY, posZ);
    }

    //function onSelectChange() {
    //    // Get model from View
    //    var selectList = document.getElementById('chooseStackingOption');
    //    var selectedConfig = selectList.options[selectList.selectedIndex].value;

    //    inputPalletSize = new Coordinates(100, 15, 150);
    //    inputBoxSize = new Coordinates(19, 10, 14);
    //    inputRowsPerLayer = 5;
    //    inputColumnsPerLayer = 10;
    //    inputLayersQuantity = 7;
    //}

    init();
    animate();
}//);