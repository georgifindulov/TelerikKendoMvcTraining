function onSave(e) {
    if (!validate(e)) {
        e.preventDefault();
        return;
    }
}

function validate(e) {
    if (e.event.CourseId === 0) {
        alert("Please select a course.");

        return false;
    }

    return true;
}