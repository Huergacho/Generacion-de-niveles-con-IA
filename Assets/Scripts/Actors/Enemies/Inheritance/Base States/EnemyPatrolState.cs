using System;
using UnityEngine;

public class EnemyPatrolState<T> : State<T>
{
    private IArtificialMovement _ia;
    private INode _root;
    private SteeringType _obsEnum;
    private int currentPosition = 0;
    private bool isDoingReverse = false;

    public EnemyPatrolState(IArtificialMovement ia, INode root, SteeringType obsEnum)
    {
        _ia = ia;
        _root = root;
        _obsEnum = obsEnum;
    }

    public override void Awake()
    {
        _ia.LifeController.OnTakeDamage += TakeHit;
        _ia.Avoidance.SetActualBehaviour(_obsEnum);
    }

    public override void Execute()
    {
        if (_ia.IsTargetInSight())
        {
            _root.Execute();
            return;
        }

        Movement();
        return;
    }

    private void Movement()
    {
        Vector3 currentTarget = _ia.PatrolRoute[currentPosition].transform.position;
        Vector3 dir = (currentTarget - _ia.transform.position).normalized;
        _ia.Move(dir, _ia.ActorStats.RunSpeed);
        _ia.LookDir(dir);   

        var distance = Vector3.Distance(_ia.transform.position, currentTarget);
        if (distance <= 1f)
        {
            ChangeCurrentPosition();
            //_root.Execute(); //Esto es si queremos que al llegar a cada waypoint recorra de nuevo el behaveiour tree (lo usaba para generar random animations en el tp1)
        }
    }

    private void ChangeCurrentPosition()
    {
        if (!isDoingReverse) //Si no hace reverse, esto va a dar siempre falso!
        {
            if (currentPosition < _ia.PatrolRoute.Length - 1) //Si es menor al total, sumale
            {
                currentPosition++;
            }
            else
            {
                if (_ia.IAStats.CanReversePatrol) //Si ya llego al final... Fijate si puede revertir
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
        _ia.TakeHit();
        _root.Execute();
    }

    public override void Sleep()
    {
        _ia.LifeController.OnTakeDamage -= TakeHit;
    }
}
