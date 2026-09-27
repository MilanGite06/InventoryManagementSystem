const CACHE_NAME = "inventory-ms-cache-v1";
const urlsToCache = [
    "/Dashboard/Index",
    "/css/site.css"
];

// Install event - cache key resources
self.addEventListener("install", (event) => {
    event.waitUntil(
        caches.open(CACHE_NAME).then((cache) => {
            return Promise.all(
                urlsToCache.map((url) => {
                    return fetch(url)
                        .then((response) => {
                            if (response.ok) {
                                return cache.put(url, response);
                            }
                        })
                        .catch((err) => console.log("Failed to cache:", url, err));
                })
            );
        })
    );
    self.skipWaiting();
});

// Activate event - clean up old caches
self.addEventListener("activate", (event) => {
    event.waitUntil(
        caches.keys().then((cacheNames) => {
            return Promise.all(
                cacheNames
                    .filter((name) => name !== CACHE_NAME)
                    .map((name) => caches.delete(name))
            );
        })
    );
    self.clients.claim();
});

// Fetch event - serve from cache if offline, otherwise fetch from network
self.addEventListener("fetch", (event) => {
    event.respondWith(
        fetch(event.request)
            .then((response) => {
                if (event.request.method === "GET" && response.status === 200) {
                    const responseClone = response.clone();
                    caches.open(CACHE_NAME).then((cache) => {
                        cache.put(event.request, responseClone);
                    });
                }
                return response;
            })
            .catch(() => {
                return caches.match(event.request);
            })
    );
});