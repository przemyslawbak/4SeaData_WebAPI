﻿using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public interface IEFRepository
    {
        List<SeaModel> GetAllSeaAreas();
        List<VesselAisUpdateModel> GetAllVesselsUpdateModels();
        void SaveDatabaseQuantities();
    }
}