// This function is invoked only when the form is valid.
function onFormSubmit(e) {
    const form = $(e.sender.element);

    const submitBtn = form.find('button[type="submit"]');
    submitBtn.prop("disabled", true);

    $.ajax({
        url: '/Home/DashboardInfo',
        type: 'GET',
        success: function (data) {
            submitBtn.prop("disabled", false);
        },
        error: function () {
            submitBtn.prop("disabled", false);
        }
    });

    // Be careful here because if the Kendo form buttons template is be overwritten, you may not have a `Clear` button.
    // Reference: https://www.telerik.com/aspnet-core-ui/documentation/html-helpers/layout/form/buttons#setting-the-buttons-template
    const clearBtn = form.find('.k-form-clear');

    if (clearBtn.length) {
        clearBtn.prop("disabled", true);
    }
}