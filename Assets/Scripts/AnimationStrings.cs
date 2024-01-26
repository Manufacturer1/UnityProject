using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 *Acest cod definește o clasă numită AnimationStrings, care conține câteva câmpuri statice de tip șir de caractere (string).
 *Aceste șiruri sunt utilizate pentru a referenția numele parametrilor de animație într-un sistem de animație Unity. 
 *Fiecare șir are o semnificație specifică în cadrul animației și este utilizat pentru a accesa sau seta diverse stări ale animatorului. 
 **/
class AnimationStrings
{
    internal static string isMoving = "isMoving";  // Animatie pentru miscare
    internal static string isRunning = "isRunning";  // Animatie pentru alergare
    internal static string isGrounded = "isGrounded";  // Animatie pentru contactul cu solul
    internal static string yVelocity = "yVelocity";  // Animatie pentru viteza pe axa Y
    internal static string jump = "jump";  // Animatie pentru saritura
    internal static string isOnCeiling = "isOnWall";  // Animatie pentru contactul cu tavanul
    internal static string isOnWall = "isOnCeiling";  // Animatie pentru contactul cu peretele
    internal static string attack = "attack";  // Animatie pentru atac
    internal static string canMove = "canMove";  // Animatie pentru permisiunea de a se deplasa
    internal static string hasTarget = "hasTarget";  // Animatie pentru existența unei ținte
    internal static string isAlive = "isAlive";  // Animatie pentru stare de viu
    internal static string isHit = "isHit";  // Animatie pentru impact
    internal static string hit = "hit";  // Animatie pentru lovitura
    internal static string lockVelocity = "lockVelocity";  // Animatie pentru blocarea vitezei
    internal static string attackCoolDown = "attackCoolDown";  // Animatie pentru timpul de așteptare între atacuri
    internal static string rangedAttackTrigger = "rangedAttack";  // Animatie pentru atac la distanță
    internal static string shieldDefend = "shieldDefend";  // Animatie pentru apărare cu scut
}

