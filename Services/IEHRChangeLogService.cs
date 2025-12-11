using clinical.APIs.Models;
using System.Security.Claims;

namespace clinical.APIs.Services
{
    public interface IEHRChangeLogService
    {
        Task LogChangesAsync(EHR oldEhr, EHR newEhr, int doctorId, string doctorName, int appointmentId);
        Task LogCreationAsync(EHR ehr, int doctorId, string doctorName, int appointmentId);
    }
}
