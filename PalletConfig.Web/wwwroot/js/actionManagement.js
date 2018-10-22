$(document).ready(function () {
    $("[data-pdsa-action]").on("click", function (e) {
        e.preventDefault();
        $("#EventCommand").val($(this).data("pdsa-action"));
        $("form").submit();
    });

    $("#drawBtn").on("click", function (e) {
        e.preventDefault();
        var selectId = $("#chooseStackingOption option:selected").val();

        alert(jsonConfig[selectId].OptionName);

        inputPalletSize = new Coordinates(jsonPallet.PalletSizeX, jsonPallet.PalletSizeY, jsonPallet.PalletSizeZ);
        inputBoxSize = new Coordinates(jsonPallet.BoxSizeX, jsonPallet.BoxSizeY, jsonPallet.BoxSizeZ);
        inputRowsPerLayer = jsonConfig[selectId].RowsPerLayer;
        inputColumnsPerLayer = jsonConfig[selectId].ColumnsPerLayer;
        inputLayersQuantity = jsonConfig[selectId].LayersQuantity;

        draw3D();
    });
});

