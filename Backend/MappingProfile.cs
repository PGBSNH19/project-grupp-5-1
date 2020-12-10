﻿using AutoMapper;
using Backend.DTO;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to DTO
            CreateMap<Weather, WeatherToUpdateDto>();

            // DTO to domain
            CreateMap<WeatherToUpdateDto, Weather>();
        }
    }
}