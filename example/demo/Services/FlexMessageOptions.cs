namespace Demo.Services;

// 옵션 클래스. IsFileMessageWriteLiveView 프로퍼티로 파일 메시지 기록 라이브 뷰 여부를 설정할 수 있음.
// Option class. Can set whether to file message write live view with IsLiveViewFileMessageWrite property.
public class FlexMessageOption
{
    public FileMessageStatus FileMessageStatus { get; set; }
}

public enum FileMessageStatus
{
    LiveView,
    Off
}