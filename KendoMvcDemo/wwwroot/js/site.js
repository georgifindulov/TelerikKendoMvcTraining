function startLoader(loaderId) {
    var loaderReference = $(loaderId).data("kendoLoader");
    loaderReference.show();
}

function stopLoader(loaderId) {
    var loaderReference = $(loaderId).data("kendoLoader");
    loaderReference.hide();
}