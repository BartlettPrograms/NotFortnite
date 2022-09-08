using System.Collections.Generic;
using UnityEngine;

public enum SMBTiming { OnEnter, OnExit, OnUpdate, OnEnd }
public class SMB_Event : StateMachineBehaviour
{
    
    [System.Serializable]
    public class SMBEvent
    {
        public bool fired;
        public string eventName;
        public SMBTiming timing;
        public float onUpdateFrame = 1;
    }
    
    [SerializeField] private int m_totalFrames;
    [SerializeField] private int m_currentFrame;
    [SerializeField] private float m_normalizedTime;
    [SerializeField] private float m_normalizedTimeUncapped;
    [SerializeField] private string m_motionTime = "";
    
    public List<SMBEvent> Events = new List<SMBEvent>();

    private bool m_hasParam;
    private Comp_SMBEventCurrator m_eventCurrator;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_hasParam = HasParameter(animator, m_motionTime);
        m_eventCurrator = animator.GetComponent<Comp_SMBEventCurrator>();
        m_totalFrames = GetTotalFrames(animator, layerIndex);

        m_normalizedTimeUncapped = stateInfo.normalizedTime;
        m_normalizedTime = m_hasParam ? animator.GetFloat(m_motionTime) : GetNormalizedTime(stateInfo);
        m_currentFrame = GetCurrentFrame(m_totalFrames, m_normalizedTime);
        if(m_eventCurrator != null)
            foreach (SMBEvent _smbEvent in Events)
            {
                _smbEvent.fired = false;
                if (_smbEvent.timing == SMBTiming.OnEnter)
                {
                    _smbEvent.fired = true;
                    m_eventCurrator.Event.Invoke(_smbEvent.eventName);
                }
            }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_normalizedTimeUncapped = stateInfo.normalizedTime;
        m_normalizedTime = m_hasParam ? animator.GetFloat(m_motionTime) : GetNormalizedTime(stateInfo);
        m_currentFrame = GetCurrentFrame(m_totalFrames, m_normalizedTime);
        if (m_eventCurrator != null)
            foreach (SMBEvent _smbEvent in Events)
                if (!_smbEvent.fired)
                {
                    if (_smbEvent.timing == SMBTiming.OnUpdate)
                    {
                        //Debug.Log(m_currentFrame + "   >=   " + _smbEvent.onUpdateFrame);
                        if (m_currentFrame >= _smbEvent.onUpdateFrame)
                        {
                            _smbEvent.fired = true;
                            m_eventCurrator.Event.Invoke(_smbEvent.eventName);
                        }
                    }
                    else if (_smbEvent.timing == SMBTiming.OnEnd)
                    {
                        if (m_currentFrame >= m_totalFrames)
                        {
                            _smbEvent.fired = true;
                            m_eventCurrator.Event.Invoke(_smbEvent.eventName);
                        }
                    }
                }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_eventCurrator != null)
        {
            foreach (SMBEvent _smbEvent in Events)
                if (_smbEvent.timing == SMBTiming.OnExit)
                {
                    _smbEvent.fired = true;
                    m_eventCurrator.Event.Invoke(_smbEvent.eventName);
                    //Debug.Log(_smbEvent.eventName);
                }
        }
    }

    private bool HasParameter(Animator animator, string parameterName)
    {
        if (string.IsNullOrEmpty(parameterName) || string.IsNullOrWhiteSpace(parameterName))
            return false;
        
        foreach (AnimatorControllerParameter parameter in animator.parameters)
            if (parameter.name == parameterName)
                return true;

        return false;
    }

    private int GetTotalFrames(Animator animator, int layerIndex)
    {
        AnimatorClipInfo[] _clipInfos = animator.GetNextAnimatorClipInfo(layerIndex);
        if(_clipInfos.Length == 0)
            _clipInfos = animator.GetCurrentAnimatorClipInfo(layerIndex);
        
        AnimationClip _clip = _clipInfos[0].clip;
        return Mathf.RoundToInt(_clip.length * _clip.frameRate);
    }

    private float GetNormalizedTime(AnimatorStateInfo stateInfo)
    {
        return stateInfo.normalizedTime > 1 ? 1 : stateInfo.normalizedTime;
    }

    private int GetCurrentFrame(int totalFrames, float normalizedTime)
    {
        return Mathf.RoundToInt(totalFrames * normalizedTime);
    }
}
