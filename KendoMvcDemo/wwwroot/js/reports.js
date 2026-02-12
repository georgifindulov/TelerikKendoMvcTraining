$(document).ready(function () {
	const reportName = $('#reportName').val();

	$("#reportViewer")
		.telerik_ReportViewer({
			serviceUrl: "api/reports/",
			reportSource: {
				report: reportName,
				parameters: {
					PARAM_Courses: [1, 3, 5]
				}
			},
			viewMode: telerikReportViewer.ViewModes.INTERACTIVE,
			scaleMode: telerikReportViewer.ScaleModes.SPECIFIC,
			scale: 1.0,
			enableAccessibility: false,
			sendEmail: { enabled: true }
		});
});