// TODO: clear all the unnecessary code and improve some of the functions

var map;
var geocoder;
var directionsService;
var directionsDisplay;

var origin;
var destination;
var originInputCoordinates;
var destinationInputCoordinates;

var infoWindow;
var marker;
var clicker = 0;
var infoHandler;
var DistanceBearingPolyline;
var isDistanceFlag = false;
var DistBearingPolylineClickHdl = 0;
var DistBearingMapClickHdl = 0;
var InfoWindowCloseClickHdl = 0;

var originInput = document.getElementById("from_places");
var destinationInput = document.getElementById("to_places");


// Getting Origin Address With Marker
function getOriginAdressWithPin(geocoder, map, infowindow) {
    const input = document.getElementById("latlng").value;
    const latlngStr = input.split(",", 2);
    latlngStr[0] = latlngStr[0].substring(1); // Removing "(" in order to parse the coordinates 
    latlngStr[1] = latlngStr[1].slice(0, -1); // Removing ")" in order to parse the coordinates 
    const latlng = {
        lat: parseFloat(latlngStr[0]),
        lng: parseFloat(latlngStr[1]),
    };
    geocoder.geocode({ location: latlng }, (results, status) => {
        if (status === "OK") {
            if (results[0]) {
                map.setZoom(12);
                infowindow.setContent(results[0].formatted_address);
                originInput.value = infowindow.content;
                infowindow.open(map);
            } else {
                window.alert("No results found");
            }
        } else {
            window.alert("Geocoder failed due to: " + status);
        }
    });
}

// Getting Destination Address With Marker
function getDestinationAdressWithPin(geocoder, map, infowindow) {
    const input = document.getElementById("latlng2").value;
    const latlngStr = input.split(",", 2);
    latlngStr[0] = latlngStr[0].substring(1); // Removing "(" in order to parse the coordinates 
    latlngStr[1] = latlngStr[1].slice(0, -1); // Removing ")" in order to parse the coordinates 
    const latlng = {
        lat: parseFloat(latlngStr[0]),
        lng: parseFloat(latlngStr[1]),
    };
    geocoder.geocode({ location: latlng }, (results, status) => {
        if (status === "OK") {
            if (results[0]) {
                map.setZoom(12);
                infowindow.setContent(results[0].formatted_address);
                destinationInput.value = infowindow.content;
                infowindow.open(map);
            } else {
                window.alert("No results found");
            }
        } else {
            window.alert("Geocoder failed due to: " + status);
        }
    });
}

// Calculate and Display
function calculateAndDisplayRoute(directionsService, directionsDisplay) {
    directionsService.route({
        origin: document.getElementById("from_places").value,
        destination: document.getElementById('to_places').value,
        travelMode: 'DRIVING'
    }, function (response, status) {
        if (status === 'OK') {
            directionsDisplay.setDirections(response);
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });
}

// add input listeners for places autocomplete recomendations
google.maps.event.addDomListener(window, 'load', function () {
    var from_places = new google.maps.places.Autocomplete(document.getElementById('from_places'));
    var to_places = new google.maps.places.Autocomplete(document.getElementById('to_places'));

    google.maps.event.addListener(from_places, 'place_changed', function () {
        var from_place = from_places.getPlace();
        var from_address = from_place.formatted_address;
        $('#origin').val(from_address);
    });

    google.maps.event.addListener(to_places, 'place_changed', function () {
        var to_place = to_places.getPlace();
        var to_address = to_place.formatted_address;
        $('#destination').val(to_address);
    });

});

// calculate distance
function calculateDistance() {
    var origin = $('#from_places').val();
    var destination = $('#to_places').val();
    var service = new google.maps.DistanceMatrixService();
    service.getDistanceMatrix(
        {
            origins: [origin],
            destinations: [destination],
            travelMode: google.maps.TravelMode.DRIVING,
            unitSystem: google.maps.UnitSystem.IMPERIAL, // miles and feet.
            unitSystem: google.maps.UnitSystem.METRIC, // kilometers and meters.
            avoidHighways: false,
            avoidTolls: false
        }, callback);
}

// get distance results
function callback(response, status) {
    if (status != google.maps.DistanceMatrixStatus.OK) {
        $('#result').html(err);
    } else {
        var origin = response.originAddresses[0];
        var destination = response.destinationAddresses[0];
        if (response.rows[0].elements[0].status === "ZERO_RESULTS") {
            $('#result').html("Better get on a plane. There are no roads between " + origin + " and " + destination);
        } else {
            var distance = response.rows[0].elements[0].distance;
            var duration = response.rows[0].elements[0].duration;
            console.log(response.rows[0].elements[0].distance);
            var distance_in_kilo = distance.value / 1000; // the kilom
            var distance_in_mile = distance.value / 1609.34; // the mile
            var duration_text = duration.text;
            var duration_value = duration.value;
            $('#in_mile').text(distance_in_mile.toFixed(2));
            $('#in_kilo').text(distance_in_kilo.toFixed(2));
            $('#duration_text').text(duration_text);
            $('#duration_value').text(duration_value);
            $('#from').text(origin);
            $('#to').text(destination);
        }
    }
}

// print results on submit the form
$('#distance_form').submit(function (e) {
    e.preventDefault();
    calculateDistance();
});

function initialize() {

    geocoder = new google.maps.Geocoder();
    directionsService = new google.maps.DirectionsService;
    directionsDisplay = new google.maps.DirectionsRenderer;
    const myLatlng = { lat: 42.698334, lng: 23.319941 }; // center points of the map (Selected Sofia,Builgaria)
    var mapOptions = {
        zoom: 12,
        center: myLatlng,     // Set the Latitude and longitude of the location
        mapTypeId: google.maps.MapTypeId.ROADMAP    // Set Map Type Here ROADMAP, TERRAIN, SATELLITE
    };

    map = new google.maps.Map(document.getElementById('map'),      // Creating the map object to desplay
        mapOptions);

    var onChangeHandler = function () {
        calculateAndDisplayRoute(directionsService, directionsDisplay);
    };

    directionsDisplay.setMap(map);
    origin = document.getElementById("from_places").addEventListener("change", onChangeHandler);
    destination = document.getElementById("to_places").addEventListener("change", onChangeHandler);

    infoWindow = new google.maps.InfoWindow();
    const locationButton = document.getElementById("current_location_button");
    locationButton.textContent = "Pan to Current Location";

    origin = new google.maps.Marker({
        position: 0,
        map,
    });

    destination = new google.maps.Marker({
        position: 0,
        map,
    });

    originInputCoordinates = document.getElementById("latlng");
    destinationInputCoordinates = document.getElementById("latlng2");
    map.addListener("click", (mapsMouseEvent) => {

        if (clicker % 2 === 0 || clicker === 0) { 
            origin.setMap(null); // Removing last placed Marker for Point A
            origin = new google.maps.Marker({
                position: mapsMouseEvent.latLng,
                map,
                draggable: true,
                label: "A",
                animation: google.maps.Animation.DROP,
                title: "Origin point Selected!",
            });

            originInputCoordinates.value = origin.position;

            getOriginAdressWithPin(geocoder, map, infoWindow);

        } else {
            destination.setMap(null); // Removing last placed Marker for Point B
            destination = new google.maps.Marker({
                position: mapsMouseEvent.latLng,
                map,
                draggable: true,
                label: "B",
                animation: google.maps.Animation.DROP,
                title: "Destination point Selected!",
            });

            destinationInputCoordinates.value = destination.position;

            getDestinationAdressWithPin(geocoder, map, infoWindow);
        }
        ++clicker;
    });


    // Get Current Location Script
    locationButton.addEventListener("click", () => {
        // Try HTML5 geolocation.
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude,
                    };

                    origin.setMap(null); // Remove if there is previously set Marker
                    origin = new google.maps.Marker({
                        position: pos,
                        title: 'A',
                        animation: google.maps.Animation.DROP,
                        map,
                        draggable: true,
                        title: "Location found"
                    });

                    originInputCoordinates.value = origin.position;

                    getOriginAdressWithPin(geocoder, map, infoWindow);

                    infoWindow.open(map);
                    map.setCenter(pos);
                },
                () => {
                    handleLocationError(true, infoWindow, map.getCenter());
                }
            );
        } else {
            // Browser doesn't support Geolocation
            handleLocationError(false, infoWindow, map.getCenter());
        }
    });

    /*Global mousemove event, developer has to manage the event while coding to use flags for different functionalities*/
    google.maps.event.addListener(map, 'mousemove', function (event) {
        // $('#divCoords').html(roundNumber(event.latLng.lat(), 5) + "," + roundNumber(event.latLng.lng(), 5));
        if (isDistanceFlag) {
            if (DistanceBearingPolyline != null) {
                var path = DistanceBearingPolyline.getPath();
                var len = path.getLength();
                $('#divLength').html("<span>Length: " + DistanceBearingPolyline.inKm() + " km</span>");
                if (len == 1) {
                    path.push(event.latLng);
                } else {
                    path.setAt(len - 1, event.latLng);
                }
            }
        }
    });

    // Attaches an info window to a marker with the provided message. When the
    // marker is clicked, the info window will open with the secret message.
    //function attachSecretMessage(marker, secretMessage) {
    //    const infowindow = new google.maps.InfoWindow({
    //        content: secretMessage,
    //    });
    //    marker.addListener("click", () => {
    //        infowindow.open(marker.get("map"), marker);
    //    });
    //}

    //function geocodePosition(pos) {
    //    geocoder.geocode({
    //        latLng: pos
    //    }, function (responses) {
    //        if (responses && responses.length > 0) {
    //            marker.formatted_address = responses[0].formatted_address;
    //        } else {
    //            marker.formatted_address = 'Cannot determine address at this location.';
    //        }
    //        infowindow.setContent(marker.formatted_address + "<br>coordinates: " + marker.getPosition().toUrlValue(6));
    //        infowindow.open(map, marker);
    //    });
    //}

    //function codeAddress() {
    //    var address = document.getElementById('from_places').value;
    //    geocoder.geocode({
    //        'from_places': address
    //    }, function (results, status) {
    //        if (status == google.maps.GeocoderStatus.OK) {
    //            map.setCenter(results[0].geometry.location);
    //            if (marker) {
    //                marker.setMap(null);
    //                if (infowindow) infowindow.close();
    //            }
    //            marker = new google.maps.Marker({
    //                map: map,
    //                draggable: true,
    //                position: results[0].geometry.location
    //            });
    //            google.maps.event.addListener(marker, 'dragend', function () {
    //                geocodePosition(marker.getPosition());
    //            });
    //            google.maps.event.addListener(marker, 'click', function () {
    //                if (marker.formatted_address) {
    //                    infowindow.setContent(marker.formatted_address + "<br>coordinates: " + marker.getPosition().toUrlValue(6));
    //                } else {
    //                    infowindow.setContent(address + "<br>coordinates: " + marker.getPosition().toUrlValue(6));
    //                }
    //                infowindow.open(map, marker);
    //            });
    //            google.maps.event.trigger(marker, 'click');
    //        } else {
    //            alert('Geocode was not successful for the following reason: ' + status);
    //        }
    //    });
    //}
}