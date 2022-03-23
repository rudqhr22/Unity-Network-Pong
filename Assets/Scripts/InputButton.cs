using UnityEngine;
/*photon view
- 게임 오브젝트를 네트워크상에서 식별하는 방법
- 컴포넌트들의 값을 네트워크로 넘어 로컬-리모트 사이에서 동기화
- view id 동일하게 되고 동일한 형태로 동기화시킴
- 네트워크 식별하기 위해 필요
- 리스트에 할당된 변화를 관측

photon tranform view
- 보일게임 오브젝트을 해당 항목을 감지
- 항목은 photon view에서 관측*/
public class InputButton : MonoBehaviour
{
    public static float VerticalInput;

    public enum State
    {
        None,
        Down,
        Up
    }

    private State state = State.None;

    private void Update()
    {
        if(state == State.None)
        {
            VerticalInput = 0;
        }
        else if (state == State.Up)
        {
            VerticalInput = 1f;
        }
        else if (state == State.Down)
        {
            VerticalInput = -1f;
        }
    }

    public void OnMoveUpButtonPressed()
    {
        state = State.Up;
    }

    public void OnMoveUpButtonUp()
    {
        if(state == State.Up) state = State.None;
    }

    public void OnMoveDownButtonPressed()
    {
        state = State.Down;
    }

    public void OnMoveDownButtonUp()
    {
        if (state == State.Down) state = State.None;
    }
}