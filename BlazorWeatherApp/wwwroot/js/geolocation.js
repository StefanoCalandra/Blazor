window.appGeo = window.appGeo || {
    getCurrentPosition: () => {
        return new Promise((resolve, reject) => {
            if (!('geolocation' in navigator)) {
                reject('La geolocalizzazione non è supportata da questo browser.');
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
                            reject('Il permesso di accedere alla posizione è stato negato.');
                            break;
                        case error.POSITION_UNAVAILABLE:
                            reject('Le informazioni sulla posizione non sono disponibili.');
                            break;
                        case error.TIMEOUT:
                            reject('Tempo scaduto durante il recupero della posizione.');
                            break;
                        default:
                            reject('Impossibile determinare la tua posizione.');
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
