using Microsoft.AspNet.Authorization;

namespace Authentication.Policies
{
    public class StatusRequirement : AuthorizationHandler<StatusRequirement>, IAuthorizationRequirement
    {
        private readonly string _status;
        private readonly string _department;

        public StatusRequirement(string department, string status, bool isSupervisorAllowed = true)
        {
            _department = department;
            _status = status;
        }

        protected override void Handle(AuthorizationContext context, StatusRequirement requirement)
        {
            if (context.User.IsInRole("supervisor"))
            {
                context.Succeed(requirement);
                return;
            }

            if (context.User.HasClaim("department", _department) &&
                context.User.HasClaim("status", _status))
            {
                context.Succeed(requirement);
            }
        }
    }

    public static class StatusPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireStatus(this AuthorizationPolicyBuilder builder, string department, string status)
        {
            builder.AddRequirements(new StatusRequirement(department, status));
            return builder;
        }
    }
}