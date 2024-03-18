namespace TicketNow.Infra.CrossCutting.Notifications;

public static class StaticNotifications
{
    #region [Users]
    public static Notification InvalidCredentials = new Notification("InvalidCredentials", "Credenciais invalidas!");
    public static Notification UserAlreadyExists = new Notification("UserAlreadyExists", "Usuario já cadastrado!");
    public static Notification UserNotFound = new Notification("InvalidUser", "Usuario não encontrado!");
    public static Notification RevokeToken = new Notification("RevokeToken", "Token revogado com sucesso!");
    public static Notification InvalidToken = new Notification("InvalidToken", "Token invalido!");
    public static Notification UserCreated = new Notification("UserCreated", "Usuario criado com sucesso!");
    public static Notification UsernameAlreadyExists = new Notification("UsernameAlreadyExists", "Username já está sendo utilizado!");
    public static Notification UserEdited = new Notification("UserEdited", "Usuario editado com sucesso!");
    public static Notification PasswordChanged = new Notification("PasswordChanged", "Senha alterada com sucesso!");
    public static Notification PhotoUploaded = new Notification("PhotoUploaded", "Upload da foto realizado com sucesso!");
    public static Notification UserDeleted = new Notification("UserDeleted", "Usuario removido com sucesso!");
    public static Notification UserActivated = new Notification("UserActivated", "Ativação de usuário alterada com sucesso!");
    public static Notification UserApproved = new Notification("UserApproved", "Usuario aprovado com sucesso!");
    #endregion

    #region [Event]
    public static Notification EventAlreadyExists = new Notification("EventAlreadyExists", "Evento já cadastrado!");
    public static Notification EventUpdated = new Notification("EventUpdated", "Evento editado com sucesso!");
    public static Notification EventNotFound = new Notification("EventNotFound", "Evento não encontrado!");
    public static Notification EventCreated = new Notification("EventCreated", "Evento criado com sucesso!");
    public static Notification InvalidPromoter = new Notification("InvalidPromoter", "Id do promoter inválido");
    public static Notification EventAlreadyActiveOrInactive = new Notification("EventAlreadyActiveOrInactive", "Evento já está {0}");
    public static Notification EventState = new Notification("EventState", "Evento {0} com sucesso!");
    public static Notification EventDeleted = new Notification("EventDeleted", "Evento deletado com sucesso!");
    public static Notification EventDeletedConflict = new Notification("EventDeletedConflict", "Evento está ligado à um pedido, erro ao deletar");
    public static Notification EventApproved = new Notification("EventApproved", "Evento aprovado com sucesso");
    public static Notification EventContainsOrderActive = new Notification("EventContainsOrderActive", "Evento contêm pedidos ativos.");
    public static Notification EventDontContainsOrderActive = new Notification("EventDontContainsOrderActive", "Evento não contêm pedidos ativos.");

    #endregion

    #region [Order]
    public static Notification OrderSucess = new("OrderSucess", "Pedido efetuado com sucesso!");
    public static Notification OrderSucessWaitingPayment = new("OrderSucessWaitingPayment", "Pedido efetuado, aguardando pagamento.");
    public static Notification OrderError = new("OrderSucessWaitingPayment", "Pedido com erro, por gentileza refaça o processo.");
    public static Notification OrderSucessButPaymentUnauthorized = new("OrderSucessButPaymentUnauthorized", "Pagamento não autorizado.");
    public static Notification PaymentsNotificationProcessSucess = new("PaymentsNotificationProcessSucess", "Notificação de pagamentos processada com sucesso!");
    public static Notification CancelOrderByUserOrderNotFound = new("CancelOrderByUserEventNotFound", "Pedido não encontrado.");
    public static Notification CancelOrderByUserEventAlreadyCanceled = new("CancelOrderByUserEventAlreadyCanceled", "Evento já cancelado.");
    public static Notification CancelOrderByUserOrderAlreadyCanceled = new("CancelOrderByUserOrderAlreadyCanceled", "Pedido já cancelado.");
    public static Notification OrderCanceledSucess = new("OrderCanceledSucess", "Pedido cancelado com sucesso.");
    public static Notification OrderNotFound = new("OrderNotFound", "Nenhum pedido foi encontrado");
    public static Notification SendPaymentsToProcessQueueu = new("SendPaymentsToProcessQueueu", "Pagamento em processamento.");

    #endregion
}