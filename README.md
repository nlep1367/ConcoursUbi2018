# Concours universitaire Ubisoft 2018

## Installation et configuration initiale de Git en local

1) Creer un dossier en local pour votre repertoire Git local
```
cd Document
mkdir Clone
cd Clone
```

2) Cloner le répertoire en local
```
git clone https://github.com/nlep1367/ConcoursUbi2018.git
```

2.1) Accéder au dossier ConcoursUbi2018 contenant le clône du projet
```
cd ConcoursUbi2018/
```

3) Ajouter le répertoire qui sera le upstream, i.e. nlep1367/ConcoursUbi2018
```
git remote add upstream https://github.com/nlep1367/ConcoursUbi2018.git
```

* Pour vérifier si l'upstream est bien fait, il devrait apparaître dans la liste suite à la commande.
```
git remote -v
```

4) Creer votre branche personnelle
```
git checkout -b naomi_personal
```

* Pour pousser votre nouvelle branche sur le répretoire d'origine :
```
git push -u origin naomi_personal
```

## Utilisations courantes

### Mettre à jour votre branche
* En local : la branche locale est celle que vous avez créée à partir du clône du répertoire d'origine (dans votre dossier Clone/ConcoursUbi2018).
```
git fetch origin
```

*Sur le répertoire d'origne : directement sur Github dans l'onglet Branches, vous pouvez consulter votre état par rapport à la branche Master (le défaut). Si vous en retard sur les changements dans master, mettez à jour votre branche en locale (voir point précédent) et pousser les changements liés à la mise à jour (voir plus bas). Il est de votre devoir de la synchoniser avec le répertoire d'origine lorsque des changements sont y ajoutés pour éviter des conflits avec vos propres changements.

### Ajouter vos changements dans la liste des modifications de ce commit
* Pour vérifier les fichiers modifiés ajoutés (vert) et non-ajoutés (rouge) dans la liste des modifications de ce commit
```
git status
```

* Pour ajouter tous les fichiers non-ajoutés :
```
git add <file> ...
```

* Pour ajouter certains fichiers non-ajoutés :
```
git add .
```

### Créer un commit
Assurez-vous que toutes les modifications voulues sont ajoutées dans la liste des changements.
```
git status
```
Si ce n'est pas le cas, ajoutez-les!

Assurez vous de mettre un <message> résumant bien vos changements. Il sera utilisé plus tard comme référence rapide.
```
git commit -m "<message>"
```

### Pousser votre commit fait dans votre branche dans le répertoire d'origine
Dans l'exemple, je pousse mes changements dans la branche nommée naomi_personal.
```
git push
```

### Faire une Pull Request (PR) avec vos changements pour les rendres visibles à tous

!! Assurer vous que la branche sur le répertoire Github où sont les changements est à jour avec votre branche en locale. Généralement, quand vous poussez des changements sur votre branche dans le répertoire Github et que vous y accédez, ce dernier vous proposera de créer une Pull Request pour vos changements.

Aller sur le projet dans Github.

_Option 1_ : Un message dans l'entête apparait et vous propose de créer une Pull Request avec vos changements; cliquez dessus.
_Option 2_ : Le message n'apparait pas; vérifiez si votre branche dans le répertoire est à jour avec celle en locale. Si oui, cliquez sur New Pull Request.

Ajouter un titre et une description résumant vos changements apportés.

Faites reviser votre PR par un autre membre de l'équipe ; cette étape se veut un partage de la connaissance au travers de l'équipe. Le but est qu'au moins une autre personne est consciente de vos changements.
! Petit plus pour les programmeurs : la personne revisant doit s'assurer que le code revisé est clair. Si vous avez des questions ou des sugestions, dites-le! Cela permettra de s'assurer à un certain point que le code produit sera compréhensible pour les autres. Ainsi, les modifications dans ce code seront moins difficiles pour les autres qui ne sont pas l'auteur.

### Vérifier l'historique des commits
* Sur vos branches en locale pour voir l'ID du commit, son auteur et un résumé des changements apportés.
```
git log
```
_Note_ : cliquez sur q (testé sur linux though) pour sortir de la liste des logs.

_Conseil d'un collègue_ : Les logs de Git sont parfois... peu explicites. Je recommande l'utilisation de :
```
git log --all --oneline --color --graph
```

* Pour le répertoire d'origine, allez directement sur Github.

## Conseils pour les codeurs
Ajouter ce répertoire Git à votre projet dans Visual Studio (ou autre IDE que vous utiliser). La gestion des conflits devient beaucoup plus facile et vous pouvez y gérer directement votre répertoire local pour vous faciliter la vie (genre mettre automatique l'ajout pour le commit des fichiers modifiés...).
