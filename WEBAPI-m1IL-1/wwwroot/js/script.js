let jwtToken = "";

async function login() {
    const username = document.getElementById("loginUsername").value;
    const password = document.getElementById("loginPassword").value;
    const message = document.getElementById("message");

    try {
        const response = await fetch("https://localhost:7234/api/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ username, password })
        });

        if (!response.ok) {
            message.innerText = "Échec de la connexion.";
            return;
        }

        jwtToken = await response.text();
        document.getElementById("tokenDisplay").innerText = "Connexion réussie";
        message.innerText = "";
    } catch (err) {
        message.innerText = "Erreur réseau.";
    }
}

async function addAd() {
    const title = document.getElementById("title").value;
    const description = document.getElementById("description").value;
    const price = parseFloat(document.getElementById("price").value);
    const category = document.getElementById("category").value;
    const location = document.getElementById("location").value;
    const message = document.getElementById("message");

    if (!jwtToken) {
        message.innerText = "Vous devez être connecté pour ajouter une annonce.";
        return;
    }

    const ad = { title, description, price, category, location };

    try {
        const response = await fetch("https://localhost:7234/api/Ads", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${jwtToken}`
            },
            body: JSON.stringify(ad)
        });

        if (response.ok) {
            message.innerText = "Annonce ajoutée avec succès !";
            loadAds();
            message.className = "message success";
        } else {
            message.innerText = "Erreur lors de l'ajout de l'annonce.";
            message.className = "message";
        }
    } catch (err) {
        message.innerText = "Erreur réseau.";
    }
}

async function getInvoice() {
    const adId = document.getElementById("invoiceId").value;
    const message = document.getElementById("message");

    if (!jwtToken) {
        message.innerText = "Connexion requise pour obtenir une facture.";
        return;
    }

    try {
        const response = await fetch(`https://localhost:7234/api/Ads/invoice/${adId}`, {
            headers: {
                "Authorization": `Bearer ${jwtToken}`
            }
        });

        if (!response.ok) {
            message.innerText = "Impossible de récupérer la facture.";
            return;
        }

        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = `facture_${adId}.pdf`;
        document.body.appendChild(a);
        a.click();
        a.remove();
        message.innerText = "";
    } catch (err) {
        message.innerText = "Erreur lors du téléchargement de la facture.";
    }
}

async function loadAds() {
    const adsList = document.getElementById("adsList");
    adsList.innerHTML = "";

    if (!jwtToken) {
        adsList.innerText = "Veuillez vous connecter pour voir les annonces.";
        return;
    }

    try {
        const response = await fetch("https://localhost:7234/api/Ads", {
            headers: {
                "Authorization": `Bearer ${jwtToken}`
            }
        });

        if (!response.ok) {
            adsList.innerText = "Erreur lors du chargement des annonces.";
            return;
        }

        const ads = await response.json();

        if (ads.length === 0) {
            adsList.innerText = "Aucune annonce disponible.";
            return;
        }

        ads.forEach(ad => {
            const adDiv = document.createElement("div");
            adDiv.style.border = "1px solid #ccc";
            adDiv.style.padding = "10px";
            adDiv.style.marginBottom = "10px";
            adDiv.style.borderRadius = "5px";
            adDiv.innerHTML = `
                <strong>${ad.title}</strong><br>
                ${ad.description}<br>
                <em>${ad.category} - ${ad.location}</em><br>
                Prix : ${ad.price.toFixed(2)} €<br>
                <button onclick="deleteAd(${ad.id})">Supprimer</button>
                <button onclick="downloadInvoice(${ad.id})">📄 Facture</button>
            `;

            adsList.appendChild(adDiv);
        });

    } catch (err) {
        adsList.innerText = "Erreur réseau.";
    }
}

async function deleteAd(id) {
    if (!confirm("Supprimer cette annonce ?")) return;

    try {
        const response = await fetch(`https://localhost:7234/api/Ads/${id}`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${jwtToken}`
            }
        });

        if (response.ok) {
            alert("Annonce supprimée !");
            loadAds(); // Recharger la liste
        } else {
            alert("Erreur lors de la suppression.");
        }
    } catch {
        alert("Erreur réseau.");
    }
}

function downloadInvoice(adId) {
    if (!jwtToken) return;

    fetch(`https://localhost:7234/api/Ads/invoice/${adId}`, {
        headers: {
            "Authorization": `Bearer ${jwtToken}`
        }
    })
        .then(response => {
            if (!response.ok) throw new Error("Facture introuvable");
            return response.blob();
        })
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = `facture_${adId}.pdf`;
            document.body.appendChild(a);
            a.click();
            a.remove();
        })
        .catch(() => alert("Erreur lors du téléchargement de la facture."));
}