// TODO: clear all the unnecessary code and improve some of the functions

const distanceForm = document.getElementById("distance_form");
const locationButton = document.getElementById("current_location_button");
let originInputCoordinates = document.getElementById("latlng");
let destinationInputCoordinates = document.getElementById("latlng2");
let originInput = document.getElementById("from_places");
let destinationInput = document.getElementById("to_places");

let map;
let geocoder = new google.maps.Geocoder();
let matrixService = new google.maps.DistanceMatrixService();
let directionsService = new google.maps.DirectionsService;
let directionsDisplay = new google.maps.DirectionsRenderer;

let originAutocomplete = new google.maps.places.Autocomplete(document.getElementById('from_places'));
let destinationAutocomplete = new google.maps.places.Autocomplete(document.getElementById('to_places'));

var origin;
var destination;

let infoWindow = new google.maps.InfoWindow();
var marker;
var clicker = 0;
var infoHandler;
var DistanceBearingPolyline;
var isDistanceFlag = false;

const descriptorOrigin = Object.getOwnPropertyDescriptor(Object.getPrototypeOf(originInput), 'value');
const descriptorDestination = Object.getOwnPropertyDescriptor(Object.getPrototypeOf(destinationInput), 'value');

Object.defineProperty(originInput, 'value', {
    set: function (t) {
        descriptorOrigin.set.apply(this, arguments);
        calculateDistance();
    },
    get: function () {
        return descriptorOrigin.get.apply(this);
    }
});

Object.defineProperty(destinationInput, 'value', {
    set: function (t) {
        descriptorDestination.set.apply(this, arguments);
        calculateDistance();
    },
    get: function () {
        return descriptorDestination.get.apply(this);
    }
});

function initialize() {
    const centerCoordinates = { lat: 42.698334, lng: 23.319941 }; // center points of the map (Selected Sofia,Builgaria)
    var mapOptions = {
        zoom: 12,
        center: centerCoordinates,     // Set the Latitude and longitude of the location
        mapTypeId: google.maps.MapTypeId.ROADMAP,    // Set Map Type Here ROADMAP, TERRAIN, SATELLITE
        gestureHandling: "greedy", // zoming with scroll when mouse is over the map
        zoomControl: false,
    };

    map = new google.maps.Map(document.getElementById('map'),      // Creating the map object to desplay
        mapOptions);
    directionsDisplay.setMap(map);

    initializeOriginAndDestination();

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

    // Set Origin and Destination Marker
    map.addListener("click", (mapMouseEventData) => {
        if (clicker % 2 === 0 || clicker === 0) {
            origin.setMap(null); // Removing last placed Marker for Point A
            origin = new google.maps.Marker({
                position: mapMouseEventData.latLng,
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
                position: mapMouseEventData.latLng,
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
}


// Initialize origin and destiniation
function initializeOriginAndDestination() {
    origin = new google.maps.Marker({
        position: { lat: 0, lng: 0 },
        map,
    });

    destination = new google.maps.Marker({
        position: { lat: 0, lng: 0 },
        map,
    });
}

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
                    title: "A",
                    animation: google.maps.Animation.DROP,
                    map,
                    draggable: true,
                    title: "Origin point Selected!"
                });

                originInputCoordinates.value = origin.position;

                getOriginAdressWithPin(geocoder, map, infoWindow);

                infoWindow.open(map);
                map.setCenter(pos);
                clicker++;
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

// Calculate and Display
function displayRoute(directionsService, directionsDisplay) {
    origin.setMap(null);
    destination.setMap(null);
    directionsService.route({
        origin: document.getElementById("from_places").value,
        destination: document.getElementById('to_places').value,
        travelMode: 'DRIVING',
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
    google.maps.event.addListener(originAutocomplete, 'place_changed', function () {
        var from_place = originAutocomplete.getPlace();
        var from_address = from_place.formatted_address;
        $('#origin').val(from_address);
    });

    google.maps.event.addListener(destinationAutocomplete, 'place_changed', function () {
        var to_place = destinationAutocomplete.getPlace();
        var to_address = to_place.formatted_address;
        $('#destination').val(to_address);
    });

});

// calculate distance
function calculateDistance() {
    if (originInput.value === "" || destinationInput.value === "")
        return;

    displayRoute(directionsService, directionsDisplay);
    var origin = $('#from_places').val();
    var destination = $('#to_places').val();
    matrixService.getDistanceMatrix(
        {
            origins: [origin],
            destinations: [destination],
            travelMode: google.maps.TravelMode.DRIVING,
            unitSystem: google.maps.UnitSystem.IMPERIAL, // miles and feet.
            unitSystem: google.maps.UnitSystem.METRIC, // kilometers and meters.
            avoidHighways: false,
            avoidTolls: false
        }, printDistanceInfo);
}

// get distance results
function printDistanceInfo(response, status) {
    if (status != google.maps.DistanceMatrixStatus.OK) {
        $('#result').html(err);
    } else {
        if (response.rows[0].elements[0].status === "ZERO_RESULTS") {
            $('#result').html("Better get on a plane. There are no roads between " + origin + " and " + destination);
        } else {
            let distance = response.rows[0].elements[0].distance;
            let duration = response.rows[0].elements[0].duration;

            let distanceInKilometers = distance.value / 1000; // the kilom
            let distanceInMiles = distance.value / 1609.34; // the mile
            
            let durationText = duration.text;
            let durationValue = duration.value;
            let durationMS = duration.value * 1000; // Needed duration in miliseconds as spotify API returns tracks duration in MS

            $('#in_mile').text(distanceInMiles.toFixed(2));
            $('#in_kilo').text(distanceInKilometers.toFixed(2) + " km");
            $('#duration_text').text(durationText);
            $('#duration_value').text(durationValue);
            $('#duration_MS').val(durationMS);
        }
    }
}

//// print results on submit the form
//$('#distance_form').submit(function (e) {
//    e.preventDefault();
//    calculateDistance();
//});


///*Global mousemove event, developer has to manage the event while coding to use flags for different functionalities*/
    //google.maps.event.addListener(map, 'mousemove', function (event) {
    //    // $('#divCoords').html(roundNumber(event.latLng.lat(), 5) + "," + roundNumber(event.latLng.lng(), 5));
    //    if (isDistanceFlag) {
    //        if (DistanceBearingPolyline != null) {
    //            var path = DistanceBearingPolyline.getPath();
    //            var len = path.getLength();
    //            $('#divLength').html("<span>Length: " + DistanceBearingPolyline.inKm() + " km</span>");
    //            if (len == 1) {
    //                path.push(event.latLng);
    //            } else {
    //                path.setAt(len - 1, event.latLng);
    //            }
    //        }
    //    }
    //});

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





    //var onChangeHandler = function () {
    //    calculateDistance(directionsService, directionsDisplay);
    //};

    //origin = document.getElementById("from_places").addEventListener("change", onChangeHandler);
    //destination = document.getElementById("to_places").addEventListener("change", onChangeHandler);
