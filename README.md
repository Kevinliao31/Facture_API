# Projet Annonces - API et Frontend
- LIAO Kévin
- M1IL IPI
  
## Présentation du projet

Ce projet est une application web de gestion d’annonces.  
- Backend en ASP.NET Core Web API avec authentification JWT.  
- Frontend en HTML/CSS/JS pour se connecter, ajouter, supprimer des annonces et télécharger les factures au format PDF.

---

## Démarrage du projet

### Backend

1. Ouvrez le projet dans votre IDE (Visual Studio, VS Code, etc.).  
2. Lancez l’API (par défaut sur https://localhost:7234).  
3. Le Swagger est disponible à l’URL `https://localhost:7234/swagger/index.html` pour tester les endpoints.

### Frontend

1. Le frontend est disponible à l'URL `https://localhost:7234/index.html` dans un navigateur web (Chrome, Firefox, Edge).  
2. Il communique avec l’API pour authentification et gestion des annonces ainsi que génération de la facture

---

## Identifiants de connexion pour test

- **Utilisateur** : `admin`  
- **Mot de passe** : `admin123`

## Fonctionnalités

- Authentification via JWT  
- CRUD complet sur les annonces (avec token)  
- Recherche filtrée d’annonces 
- Téléchargement des factures PDF

---
