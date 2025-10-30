window.appGeo = window.appGeo || {
    getCurrentPosition: () => {
        return new Promise((resolve, reject) => {
            if (!('geolocation' in navigator)) {
                reject('Geolocation is not supported by this browser.');
                return;
            }

            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const { latitude, longitude, accuracy } = position.coords;
                    resolve({ latitude, longitude, accuracy });
                },
                (error) => {
                    switch (error.code) {
                        case error.PERMISSION_DENIED:
                            reject('Permission to access location was denied.');
                            break;
                        case error.POSITION_UNAVAILABLE:
                            reject('Location information is unavailable.');
                            break;
                        case error.TIMEOUT:
                            reject('Timed out while retrieving location.');
                            break;
                        default:
                            reject('Unable to determine your location.');
                            break;
                    }
                },
                {
                    maximumAge: 60000,
                    timeout: 10000,
                    enableHighAccuracy: true
                });
        });
    }
};
