using VeganLife.Views.Base;

namespace VeganLife.Views;

public partial class BookmarkPage : BasePage<BookmarkViewModel>
{
    public BookmarkPage(BookmarkViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}