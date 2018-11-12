// Input data
var inputPalletSize, inputBoxSize;
var layerModelStandard, layerModelRotated, inputLayersQuantity;

function Coordinates(X, Y, Z) {
    this.X = X;
    this.Y = Y;
    this.Z = Z;
    this.area = function () {
        return this.X * this.Y * this.Z;
    }
}

function LayerModel(RowsPerPallet, ColumnsPerPallet) {
    this.RowsPerPallet = RowsPerPallet;
    this.ColumnsPerPallet = ColumnsPerPallet;
    this.area = function () {
        return this.RowsPerPallet * this.ColumnsPerPallet;
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
        var canvas = document.getElementById('drawArea');
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
        generateAllLayers(layerModelStandard, layerModelRotated, zeroPosition, boxSize, inputLayersQuantity, maxPosition, areLayersOpposite);

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

    function sizeScalling(inputSize) {
        inputSize.X = (inputSize.X / inputSizeMax) * 2;
        inputSize.Y = (inputSize.Y / inputSizeMax) * 2;
        inputSize.Z = (inputSize.Z / inputSizeMax) * 2;
        return inputSize;
    }

    function generateAllLayers(layerModelStandard, layerModelRotated, zeroPosition, boxSize, layersQuantity, maxPosition, areLayersOpposite) {
        var posX = zeroPosition.X;
        var posY = zeroPosition.Y;
        var posZ = zeroPosition.Z;
        var changeSide = false;
        
        for (var i = 0; i < layersQuantity; i++) {
            // Starting position
            var position = new Coordinates(X = posX, Y = posY, Z = posZ);
            // Generate Standard part
            generateLayer(layerModelStandard.RowsPerPallet, layerModelStandard.ColumnsPerPallet, position, boxSize, changeSide, false);
            // Generate Rotated part
            generateLayer(layerModelRotated.RowsPerPallet, layerModelRotated.ColumnsPerPallet, position, boxSize, changeSide, true);
            
            // Move position up to another layer
            posY += boxSize.Y;

            // Check if user want to stack layers opposite
            if (areLayersOpposite == true) {
                if (changeSide == true) {
                    changeSide = false;
                }
                else {
                    changeSide = true;
                }

                // Change coordinates of starting position
                if (changeSide == true) {
                    posX = maxPosition.X;
                    posZ = maxPosition.Z;
                }
                else {
                    posX = zeroPosition.X;
                    posZ = zeroPosition.Z;
                }
            }
        }
    }

    function generateLayer(numberRows, numberColumns, position, boxSize, changeSide, isRotated) {
        var _boxSize = new Coordinates(boxSize.X, boxSize.Y, boxSize.Z);
        var startingPosition = new Coordinates(position.X, position.Y, position.Z);
        if (isRotated == true) {
            // Rotate box
            var helper = _boxSize.X;
            _boxSize.X = _boxSize.Z;
            _boxSize.Z = helper;

            // Align position after rotation
            if (changeSide == true) {
                startingPosition.X += (_boxSize.Y - _boxSize.Z) / 2;
                position.X = startingPosition.X;
                startingPosition.Z -= (_boxSize.Z - _boxSize.X) / 2;
            }
            else {
                startingPosition.X -= (_boxSize.Y - _boxSize.Z) / 2;
                position.X = startingPosition.X;
                startingPosition.Z += (_boxSize.Z - _boxSize.X) / 2;
            }
        }

        // Generate boxes in layer
        for (var i = 0; i < numberRows; i++) {
            position.Z = startingPosition.Z;
            for (var j = 0; j < numberColumns; j++) {
                generateBox(_boxSize, position.X, position.Y, position.Z);
                if (changeSide == true) { position.Z -= _boxSize.Z; }
                else { position.Z += _boxSize.Z; }
            }
            if (changeSide == true) { position.X -= _boxSize.X; }
            else { position.X += _boxSize.X; }
        }
        position.X = startingPosition.X;
    }

    function generateBox(boxSize, posX, posY, posZ) {
        var geometryBox = new THREE.BoxGeometry(boxSize.X, boxSize.Y, boxSize.Z);
        var meshBox = new THREE.Mesh(geometryBox, materialBox);
        scene.add(meshBox);
        meshTop.add(meshBox);
        meshBox.position.set(posX, posY, posZ);
    }

    init();
    animate();
}