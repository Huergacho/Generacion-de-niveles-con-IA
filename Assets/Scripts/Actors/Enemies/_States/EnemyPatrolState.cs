using System;
using UnityEngine;

public class EnemyPatrolState<T> : State<T>
{
    private IArtificialMovement _self;
    private INode _root;
    private SteeringType _obsEnum;
    private int currentPosition = 0;
    private bool isDoingReverse = false;
    private Vector3 currentTarget;
    private int currentRandom;

    public EnemyPatrolState(IArtificialMovement ia, INode root, SteeringType obsEnum)
    {
        _self = ia;
        _root = root;
        _obsEnum = obsEnum;
    }

    public override void Awake()
    {
        _self.LifeController.OnTakeDamage += TakeHit;
        _self.Avoidance.SetActualBehaviour(_obsEnum);
        currentRandom =_self.RamdonizeTargetInPatrolRoute();
    }

    public override void Execute()
    {
        if (_self.IsTargetInSight())
            _root.Execute();

        Movement();
    }

    private void Movement()
    {
        currentTarget = _self.PatrolRoute[currentRandom].transform.position;
        Vector3 dir = (currentTarget - _self.transform.position).normalized;
        _self.Move(dir, _self.ActorStats.RunSpeed);
        _self.LookDir(dir);   

        var distance = Vector3.Distance(_self.transform.position, currentTarget);
        if (distance <= _self.IAStats.NearTargetRange)
        {
            //ChangeCurrentPosition();
            currentRandom = _self.RamdonizeTargetInPatrolRoute();
            _root.Execute(); //Esto es si queremos que al llegar a cada waypoint recorra de nuevo el behaveiour tree (lo usaba para generar random animations en el tp1)
        }
    }

    private void ChangeCurrentPosition()
    {
        if (!isDoingReverse) //Si no hace reverse, esto va a dar siempre falso!
        {
            if (currentPosition < _self.PatrolRoute.Length - 1) //Si es menor al total, sumale
            {
                currentPosition++;
            }
            else
            {
                if (_self.IAStats.CanReversePatrol) //Si ya llego al final... Fijate si puede revertir
                {
                    isDoingReverse = true; //Activa la vuelva atras
                    currentPosition--; //Y resta una posicion
                }
                else
                {
                    currentPosition = 0; //Sino mandalo a la posicion inicial
                }
            }
        }
        else //Si esta haciendo el reverse
        {
            if (currentPosition > 0) //Y todavia no llego a 0
            {
                currentPosition--; //Segui restando
            }
            else
            {
                isDoingReverse = false; //Si llego a cero, sacalo de aca
                currentPosition++; //y ya sumale una para que valla caminando
            }
        }
    }

    private void TakeHit()
    {
        _self.TakeHit(true);
        _root.Execute();
    }

    public override void Sleep()
    {
        _self.LifeController.OnTakeDamage -= TakeHit;
    }
}
