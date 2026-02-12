function onRead(options) {
    $.ajax({
        url: $('#loadSpreadsheetDataUrl').val(),
        dataType: "json",
        success: function (result) {

            const spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
            const sheet = spreadsheet.activeSheet();

            result.Data.forEach(function (item, index) {
                const rowNumber = index + 2; // Assuming row 0 is header
                const imageId = spreadsheet.addImage(item.Avatar);    //  Generate the image to the spreadsheet's internal registry
                item.Avatar = ''; // Clear the URL to avoid displaying it as text

                const drawing = kendo.spreadsheet.Drawing.fromJSON({
                    topLeftCell: "D" + rowNumber,
                    offsetX: 60,  // Slight padding inside the cell
                    offsetY: 5,
                    width: 20,
                    height: 20,
                    image: imageId
                });

                sheet.addDrawing(drawing);
            });

            options.success(result.Data);

            var range = $("#spreadsheet").data("kendoSpreadsheet").activeSheet().range("A2:A201");
            range.enable(false);
        },
        error: function (result) {
            options.error(result);
        }
    });
}

function onDataBinding(e) {
    console.log('Data is about to be bound to sheet "' + e.sheet.name() + '".');
}

function onDataBound(e) {
    console.log('Data has been bound to sheet "' + e.sheet.name() + '".');

    var sheet = e.sender.activeSheet();
    var dataSource = sheet.dataSource;
    var dataItemCount = dataSource.view().length; // This is 200

    // 1. Calculate required rows: Data (200) + Header (1)
    var requiredTotal = dataItemCount + 1;
}