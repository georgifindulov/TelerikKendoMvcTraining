function onRefreshDashboardInfo() {
    const dashboardInfoContainer = $('#dashboardInfo');

    startLoader('#loader');

    $.ajax({
        url: '/Home/DashboardInfo',
        type: 'GET',
        success: function (data) {
            dashboardInfoContainer.html(data);
            stopLoader('#loader');
        },
        error: function () {
            stopLoader('#loader');
            alert('Error refreshing dashboard information.');
        }
    });
}