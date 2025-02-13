function preventFormSubmission(formId) {
    document.querySelector(`#${formId}`).addEventListener("keydown", function (event) {
        if (event.key === "Enter") {
            event.preventDefault();
            return false;
        }
    })
}