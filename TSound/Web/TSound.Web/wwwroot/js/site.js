
function displayBusyIndicator() {
    document.getElementById("loading").style.display = "block";
}

// Playlist profile has expandable tables.
function expandList(tableId) {
    var element = document.getElementById(tableId);
    if (element.style.visibility == "collapse") {
        element.style.visibility = "visible";
    }
    else {
        element.style.visibility = "collapse";
    }
}

function expandListPlaylists() {
    var element = document.getElementById("table-playlists");
    if (element.style.visibility == "collapse") {
        element.style.visibility = "visible";
    }
    else {
        element.style.visibility = "collapse";
    }
}

function play(elementId) {
    var btn = document.getElementById(elementId);
    var audio = document.getElementById(`audio-${elementId}`);
    audio.volume = 0.09;

    if (btn.classList.contains("paused")) {
        audio.pause();
        btn.innerHTML = `<i class="tim-icons icon-triangle-right-17"></i>`;
        btn.classList.remove("paused");
    } else {
        audio.play();
        btn.innerHTML = `<i class="tim-icons icon-button-pause"></i>`;
        btn.classList.add("paused")
    }
    return false;
}

$(function () {

    //slider for duration here

    //initialize the slider
    $('#slider-range-duration').slider({
        range: true, //has range, so 2 values - start and end
        values: [0, 24], //the 2 values
        min: 0,
        max: 24,
        slide: function (event, ui) { //the event that sets all values when slider values change
            $("#duration-min").val(ui.values[0]); //set the hidden inputs
            $("#duration-max").val(ui.values[1]);
            $("#duration-min-span").html(ui.values[0]); //set the spans on the 2 sides of the slider
            $("#duration-max-span").html(ui.values[1]);
        }
    });

    //the initial hidden input value
    $("#duration-min").val($("#slider-range-duration").slider("option", "values")[0]);
    $("#duration-max").val($("#slider-range-duration").slider("option", "values")[1]);
    //the initial span values on document load 
    $("#duration-min-span").html($("#slider-range-duration").slider("option", "values")[0]);
    $("#duration-max-span").html($("#slider-range-duration").slider("option", "values")[1]);


    //slider for rank here
    $('#slider-range-rank').slider({
        range: true,
        values: [100000, 1000000],
        min: 100000,
        max: 1000000,
        step: 1000, // Determines the size or amount of each interval or step the slider takes between the min and max. 
        slide: function (event, ui) {
            $("#rank-min").val(ui.values[0]);
            $("#rank-max").val(ui.values[1]);
            $("#rank-min-span").html(ui.values[0]);
            $("#rank-max-span").html(ui.values[1]);
        }
    });

    $("#rank-min").val($("#slider-range-rank").slider("option", "values")[0]);
    $("#rank-max").val($("#slider-range-rank").slider("option", "values")[1]);

    $("#rank-min-span").html($("#slider-range-rank").slider("option", "values")[0]);
    $("#rank-max-span").html($("#slider-range-rank").slider("option", "values")[1]);
})
