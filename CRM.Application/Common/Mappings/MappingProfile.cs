using AutoMapper;
using CRM.Application.DTOs;
using CRM.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRM.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity → DTO  
        CreateMap<Lead, LeadDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Source, o => o.MapFrom(s => s.Source.ToString()));

        CreateMap<PurchaseOrder, PurchaseOrderDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));

        CreateMap<PurchaseOrderItem, PurchaseOrderItemDto>()
            .ForMember(d => d.TotalPrice,
                o => o.MapFrom(s => s.Quantity * s.UnitPrice));
    }
}