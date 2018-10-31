$(document).ready(function () {
    $("[data-pdsa-action]").on("click", function (e) {
        e.preventDefault();
        $("#EventCommand").val($(this).data("pdsa-action"));
        $("form").submit();
    });

    $("#drawBtn").on("click", function (e) {
        e.preventDefault();

        // Selected option
        var selectId = $("#chooseStackingOption option:selected").val();

        // Pass input data to draw 3D model
        inputPalletSize = new Coordinates(jsonConfig[selectId].PalletSize.X, jsonConfig[selectId].PalletSize.Y, jsonConfig[selectId].PalletSize.Z);
        inputBoxSize = new Coordinates(jsonConfig[selectId].BoxSize.X, jsonConfig[selectId].BoxSize.Y, jsonConfig[selectId].BoxSize.Z);
        layerModelStandard = new LayerModel(jsonConfig[selectId].Standard.RowsPerLayer, jsonConfig[selectId].Standard.ColumnsPerLayer);
        layerModelRotated = new LayerModel(jsonConfig[selectId].Rotated.RowsPerLayer, jsonConfig[selectId].Rotated.ColumnsPerLayer);
        inputLayersQuantity = jsonConfig[selectId].LayersQuantity;

        // Clear canvas div
        $("#drawArea").empty();

        // Draw 3D model of pallet
        draw3D();
    });
});

