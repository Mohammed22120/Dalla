# Dalla — front-end .NET MAUI (BudgetFamille)

Application mobile de gestion de budget familial. 

## Lancer le projet dans Visual Studio

1. Installer Visual Studio avec la charge de travail **« Développement d'applications mobiles .NET (MAUI) »**.
2. Ouvrir le fichier `Dalla.csproj` (Fichier > Ouvrir > Projet/Solution).
3. Choisir la cible de débogage :
   - **Android Emulator** (créer un appareil dans Android Device Manager), ou
   - **Windows Machine** (sur Windows, pour tester sans émulateur).
4. Appuyer sur F5.

> Le projet cible `net10.0` (Visual Studio 2026). Avec Visual Studio 2022 / .NET 9, remplace `net10.0` par `net9.0`
> dans `Dalla.csproj` (3 occurrences : `TargetFrameworks` ×2 et la cible Windows).

## Se connecter (démo)

- E-mail : n'importe quelle adresse valide, 
- Mot de passe : `dalla123` (tout autre mot de passe affiche l'erreur « identifiants incorrects »)
- Code d'invitation pour « Rejoindre un foyer » : `DALLA-7K2P`

## Correspondance avec le cahier des charges

| N° | Écran | Fichier |
|----|-------|---------|
| 01 | Connexion | `Views/LoginPage` |
| 02 | Inscription (force du mot de passe, validation en direct) | `Views/RegisterPage` |
| 03 | Mot de passe oublié | `Views/ForgotPasswordPage` |
| 04 | Création / accès à un foyer | `Views/HouseholdSetupPage` |
| 05 | Accueil / tableau de bord (alertes, état vide) | `Views/HomePage` |
| 06 | Ajouter (et modifier) une dépense | `Views/AddExpensePage` |
| 07 | Liste des dépenses (recherche, filtres, squelette, vide, erreur réseau) | `Views/ExpensesPage` |
| 08 | Détail d'une dépense (suppression avec confirmation) | `Views/ExpenseDetailPage` |
| 09 | Statistiques (anneau, histogramme, courbe 6 mois) | `Views/StatsPage` |
| 10 | Budget mensuel (réservé à l'administrateur) | `Views/BudgetPage` |
| 11 | Membres du foyer + invitation | `Views/MembersPage` |
| 12 | Profil et paramètres (devise, thème, notifications, déconnexion) | `Views/SettingsPage` |
| — | Onglet Foyer (menu) et Catégories | `Views/HouseholdHubPage`, `Views/CategoriesPage` |

## Charte et composants

- Couleurs (clair, sombre, catégories) : `Resources/Styles/Colors.xaml`
- Typographie, boutons, cartes, champs et leurs états : `Resources/Styles/Styles.xaml`
- Champ de saisie (normal / actif / désactivé / erreur) : `Controls/FieldView`
- Avatar (initiales sur fond coloré) : `Controls/AvatarView` ; état vide / erreur : `Controls/EmptyState`
- Graphiques dessinés en code (légers, sans bibliothèque) et logo : `Controls/Drawables.cs`
- Icônes **Lucide** (lucide.dev, licence ISC), dessinées en vectoriel et teintables : `Controls/LucideIcons.cs` (catalogue), `Controls/LucideIcon.cs` (`<controls:LucideIcon Name="house" />`), `Controls/LucideButton.cs` (bouton-icône). Pour en ajouter une, copier les éléments de son SVG dans le dictionnaire de `LucideIcons.cs`.
- Barre de navigation inférieure flottante (Accueil · Dépenses · + · Stats · Foyer) : `Controls/BottomNavBar.cs`, placée en bas des 4 pages principales ; la barre native de Shell est masquée dans `AppShell.xaml`.
- Données d'exemple et règles (budgets, alertes 80 % / 100 %) : `Models/AppData.cs`

## Tester les états (Foyer > Profil et paramètres > Mode démo)

Simuler une erreur réseau, un foyer à un seul membre, passer en simple membre (budget désactivé),
vider les dépenses (états vides), changer le thème (clair / sombre / automatique).

## Écoconception appliquée

Fonds unis, pas de dégradé, pas d'images (icônes Lucide et logo dessinés en vectoriel, avatars = initiales),
animations limitées à un fondu de 150 ms, mode sombre, squelette de chargement statique, une seule police (celle du système).
