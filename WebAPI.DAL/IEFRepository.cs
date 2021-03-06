﻿using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public interface IEFRepository
    {
        IQueryable<SeaModel> GetAllSeaAreas();
        List<VesselAisUpdateModel> GetAllVesselsUpdateModels();
        void SaveDatabaseQuantities();
        void SaveUpdateLogs(UpdateLogModel updateLog);
        IQueryable<PortModel> GetAllPorts();
        bool VerifyIfVesselArrivedSpecificPortAndNotDeparted(int portId, int vesselId);
        void VesselDeparture(int vesselId, System.DateTime? aISLatestActivity);
        void VesselArrival(int portId, int vesselId, System.DateTime? aISLatestActivity);
        void UpdatePort(int portId, int vesselId);
        bool VerifyIfVesselArrivedAnyPortAndNotDeparted(int vesselId);
        int GetPortId(string locode);
    }
}