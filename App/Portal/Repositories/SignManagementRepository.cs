using PortalDataEntities.Entities;

namespace Portal.Repositories
{
    public class SignManagementRepository
    {
        private MemberRepository MemRepository = new MemberRepository();
        private PORTALDB PorDB = new PORTALDB();
    }
}