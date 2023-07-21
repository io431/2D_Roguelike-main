using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public float moveDistance = 1f;
    public delegate void PlayerMoveHandler();
    public event PlayerMoveHandler OnMoveFinished;

    // Animator�R���|�[�l���g�̎Q�Ƃ�ǉ�
    private Animator animator;

    private bool isMoving = false;
    private Vector3 targetPosition;

    private void Awake()
    {
        // Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnMoveFinished?.Invoke();
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput == 0 && verticalInput == 0) return;

        Vector3Int currentCell = tilemap.WorldToCell(transform.position);
        Vector3Int targetCell = currentCell + new Vector3Int(Mathf.RoundToInt(horizontalInput), Mathf.RoundToInt(verticalInput), 0);

        if (!IsWalkableTile(targetCell)) return;

        targetPosition = tilemap.GetCellCenterWorld(targetCell);

        // ���͂Ɋ�Â���Animator�̕����p�����[�^�[���X�V
        if (verticalInput > 0) animator.SetInteger("Direction", 2);      // ��
        else if (verticalInput < 0) animator.SetInteger("Direction", 0); // ��
        else if (horizontalInput > 0) animator.SetInteger("Direction", 3); // �E
        else if (horizontalInput < 0) animator.SetInteger("Direction", 1); // ��

        StartCoroutine(MovePlayer());
    }
    public bool IsWalkableTile(Vector3Int cellPosition)
    {
        // �^�C���}�b�v��̈ʒu����^�C�����擾
        TileBase tile = tilemap.GetTile(cellPosition);

        // �^�C����null�łȂ����A�����Ȃ��^�C���ł����false��Ԃ�
        return (tile != null && !tile.name.Contains("Unwalkable")); // "Unwalkable"�͕����Ȃ��^�C���̖��O�ɉ����ĕύX���Ă�������
    }

    private IEnumerator MovePlayer()
    {
        isMoving = true;

        // �ړ��A�j���[�V�����Ȃǂ̏���

        // �ړ�
        while (transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDistance * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
        OnMoveFinished?.Invoke();
    }
}
