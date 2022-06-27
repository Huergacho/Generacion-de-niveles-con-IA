using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
    public class Evade : ISteering
    {
        private IArtificialMovement _self;
        private ITarget _target;

        public Evade(IArtificialMovement self)
        {
            _self = self;
            SetTarget(_self.Target);
        }

        public void SetTarget(ITarget target)
        {
            _target = target;
        }

        public Vector3 GetDir()
        {
            var predictMult = _target.GetVel * _self.IAStats.PredictionTime;
            var dist = Vector3.Distance(_self.transform.position, _target.transform.position);
            Vector3 point = _target.transform.position + _target.GetFoward * Mathf.Clamp(predictMult, -dist, dist);
            Vector3 dir = _self.transform.position - point;
            return dir.normalized;
        }
    }
