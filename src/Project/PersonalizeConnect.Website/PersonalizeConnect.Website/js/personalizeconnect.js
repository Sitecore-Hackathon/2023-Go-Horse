function GetDatasource(experienceId, experienceValue) {
    var currentUrl = encodeURIComponent(window.location.href);
    var apiUrl = "/PersonalizeConnect?pageurl=" + currentUrl + "&experienceId=" + experienceId + "&experienceValue=" + experienceValue;

    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", apiUrl, false);
    xhttp.send();
    var result = JSON.parse(xhttp.responseText);

    return result;
}

function PopulateBlock(datasource) {
    var containerId = datasource.container;
    var container = document.querySelector('[cdp-container="' + containerId + '"]');
    if (container == undefined)
        return;

    Object.keys(datasource.fields).forEach(key => {
        var subcontainer = container.querySelector('[cdp-field="' + key + '"]');
        if (subcontainer != undefined) {
            var value = datasource.fields[key];

            if (subcontainer.tagName.toLowerCase() == "img") {
                if (value.src != "")
                    subcontainer.src = value.src;
                if (value.alt != "")
                    subcontainer.alt = value.alt;
            }
            else if (subcontainer.tagName.toLowerCase() == "a") {

                if (value.href != "")
                    subcontainer.href = value.href;
                if (value.target != "")
                    subcontainer.target = value.target;
                if (value.text != "")
                    subcontainer.innerHTML = value.text;
                if (value.title != "")
                    subcontainer.title = value.title;
                if (value.class != "")
                    subcontainer.className = value.class;
            }
            else {
                subcontainer.innerHTML = value;
            }
        }

    });
}