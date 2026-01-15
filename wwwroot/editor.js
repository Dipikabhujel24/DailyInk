window.execCmd = (command, value) => {
    document.execCommand(command, false, value);
};

window.getHtml = (el) => {
    return el ? el.innerHTML : "";
};
