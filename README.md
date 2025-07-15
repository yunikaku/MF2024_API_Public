# MF2024_API

## �T�v
MF2024_API�́A��c���E���Ə��E�����E�ہi�Z�N�V�����j�E�\��E���ގ��Ǘ��ȂǁA�I�t�B�X�^�c�ɕK�v�Ȋe������Ǘ��E���삷�邽�߂�RESTful API�ł��BC# 12.0 / .NET 8�Ŏ�������Ă��܂��B

## ��ȋ@�\
- **��c���Ǘ�**  
  �������̌����E�o�^�E�X�V�E�폜�A�ݔ��E�摜�Ǘ��A���ގ������Ǘ�
- **�\��Ǘ�**  
  �\����̌����E�o�^�E�X�V�E�폜�AQR�R�[�h�����A�g�[�N�����s
- **�����E�ہi�Z�N�V�����j�Ǘ�**  
  �����E�ۏ��̌����E�o�^�E�X�V�E�폜
- **���Ə��Ǘ�**  
  ���Ə����̌����E�o�^�E�X�V�E�폜
- **NFC�����Ǘ�**  
  NFC�f�o�C�X�̊������̊Ǘ�
- **���ގ��Ǘ�**  
  NFC�ƃf�o�C�X��p�������ގ��L�^
- **�I�v�g�A�E�g�Ǘ�**  
  �f�o�C�X�ENFC�P�ʂł̃I�v�g�A�E�g���Ǘ�
- **Discord�ʒm�A�g**  
  �\��⃁�b�Z�[�W��Discord�֒ʒm

## API�G���h�|�C���g��
- `/api/ReceptionDevice/GetReservation`  
  �\����̎擾
- `/api/RoomDevice/EnterRoom`  
  ��������
- `/api/RoomDevice/ExitRoom`  
  �ގ�����
- `/api/ReceptionDevice/DiscordSend`  
  Discord�ւ̃��b�Z�[�W���M

## �f�[�^���f��
- **Department**: �������
- **Section**: �ہi�Z�N�V�����j���
- **Room**: ��c�����
- **Reservation**: �\����
- **Device**: �f�o�C�X���
- **Nfc/Nfcallotment**: NFC�f�o�C�X�E�������
- **OptOut**: �I�v�g�A�E�g���
- **Entrants**: ���ގ����

## �Z�b�g�A�b�v
1. .NET 8 SDK���C���X�g�[��
2. �K�v��NuGet�p�b�P�[�W�𕜌�
3. `appsettings.json`��DB�ڑ����̐ݒ�
4. �}�C�O���[�V�����EDB������
5. API�v���W�F�N�g���N��

## �g�p���@
API�͔F�؁iJWT/ASP.NET Identity�j�ƃ��[���Ǘ��i��: `RoomDevice`, `Reception`�j�ɑΉ����Ă��܂��B  
�e�G���h�|�C���g��Swagger��Postman���Ńe�X�g�\�ł��B

## ���ӎ���
- �eAPI�͗�O�����E�o���f�[�V��������������Ă��܂��B
- �摜�����ɂ�ImageSharp�AQR�R�[�h�����ɂ�QRCoder�𗘗p���Ă��܂��B
- Discord�A�g�ɂ�Webhook/API�𗘗p���܂��B

## ���C�Z���X
�{�v���W�F�N�g��MIT���C�Z���X�ł��B

---

�{README�͎�v�ȋ@�\�E�\���E���p���@���Ȍ��ɂ܂Ƃ߂Ă��܂��B�ڍׂ�API�d�l�⃂�f����`�͊e�R���g���[���[�E���f���N���X�����Q�Ƃ��������B