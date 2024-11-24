function scrollToElement(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.scrollIntoView({ behavior: 'smooth'});
    }
}

function downloadFile(fileUrl, fileName) {
    const a = document.createElement("a");
    a.href = fileUrl;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}

function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
        console.log("Copied to clipboard successfully!");
    }, function (err) {
        console.error("Could not copy text: ", err);
    });
}