//using UnityEngine;
//using System.Collections;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//public class SkirmishParticipantEventArgs : EventArgs
//{
//    public SkirmishParticipantEventArgs(Guid id)
//    {
//        SessionId = id;
//    }

//    public Guid SessionId { get; private set; }
//}

//public class SkirmishParticipants : ClientServerMonoBehaviour
//{
//    [SerializeField]
//    private int maxParticipantsCount;

//    private LinkedList<Guid> candidates;
//    private LinkedList<Guid> participants;

//    public SkirmishParticipants()
//    {
//        candidates = new LinkedList<Guid>();
//        participants = new LinkedList<Guid>();
//    }

//    protected override void OnServerStart()
//    {
//        participants.AddLast(LocalSessionId);
//        OnAddParticipant.SafeRaise(this, new SkirmishParticipantEventArgs(LocalSessionId));
//    }

//    protected override void OnClientConnect(ClientConnectEventArgs args)
//    {
//        candidates.AddLast(args.ClientSessionId);
//    }

//    protected override void OnClientDisconnect(ClientDisconnectEventArgs args)
//    {
//        if(participants.Remove(args.ClientSessionId))
//        {
//            OnRemoveParticipant.SafeRaise(this, new SkirmishParticipantEventArgs(args.ClientSessionId));
//        }

//        candidates.Remove(args.ClientSessionId);
//    }

//    private void Update()
//    {
//        if(candidates.Any() && participants.Count < maxParticipantsCount)
//        {
//            var candidate = candidates.First;
//            candidates.RemoveFirst();
//            participants.AddLast(candidate);
//            OnAddParticipant.SafeRaise(this, new SkirmishParticipantEventArgs(candidate.Value));
//        }
//    }

//    public IEnumerable<Guid> Participants
//    {
//        get
//        {
//            return participants;
//        }
//    }

//    public event EventHandler<SkirmishParticipantEventArgs> OnAddParticipant;

//    public event EventHandler<SkirmishParticipantEventArgs> OnRemoveParticipant;
//}
