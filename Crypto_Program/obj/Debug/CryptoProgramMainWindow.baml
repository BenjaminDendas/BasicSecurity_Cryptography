   M S B A M L   `   `   `  ����  Crypto_Program,    &Crypto_Program.CryptoProgramMainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Crypto-Program��5   	   $��687.5��6    $	��857��6/    � WindowStartupLocation$ CenterScreen=�6T    � Icon$ lock.ico��6y    � 
ResizeMode$ CanMinimize=�6�   .��5      � .+�5   
   �� -   5         ��Name$ cryptoTabControl��5      $��0,23,0,0q�6/   $	��636��6A   $	��Top=�6N   .@�5      �� $��Encrypt File��6   $��22��6,   $��Bottom=�68   .��5      � ��5	      �� 5
       �� ImageSource$* $/Crypto_Program;component/BSFoto.jpg��6%   .+�5      �� -   5         ��Name$' !encrypt_File_BestandsLocatieLabel��5   m   $��Courier New)�6   $
��Bold"�65   $	��260��6G   $��30��6S   $��17(�6_   $��Te Encrypteren bestand:��6�   $
��Left=�6�   $��
10,224,0,0q�6�   $	��Top=�6�   �� 5      -   5         ��Name$) #encrypt_File_BestandsLocatieTextBox��5      $��Center=�6K   $��Center=�6o   $
��Left=�6�   $��30��6�   $��288,224,0,0q�6�    �� TextWrapping$
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6  	 � ToolTip$2	 ,Geef het bestand dat je wilt encrypteren op.��6  �� 5      -   5        
 ��Name$(
 "encrypt_File_BestandsLocatieButton��5      $	��...��6J   $
��Left=�6X   $��736,224,0,0q�6s   $	��Top=�6�   $��82��6�   $��30��6�   6�   �� 5      -   5        $( "encrypt_File_encryptieMethodeLabel��5      $��Courier New)�6H   $
��Bold"�6a   $��17(�6s   $��Encryptie-Methode:��6�   $
��Left=�6�   $��
159,52,0,0q�6�   $	��Top=�6�   $	��250��6�   �� 5      -   5         ��Name$$ encrypt_File_encryptieGroubBox��5      $��	Encryptie��6G   $
��Left=�6Z   $��332,114,0,0q�6u   $	��Top=�6�   $��74��6�   $	��270��6�   $��Courier New)�6�   $
��Bold"�6�   $��17(�6�   .��5      � $��	10,0,-2,0q�6   .+�5      	� -   5         	�Name$" encrypt_File_DES_RadioButton��5   *   $��DES-Encryptie��6P   $��
10,10,19,0q�6h   $	��Top=�6|   $��Courier New)�6�   $
��Bold"�6�   $	��159��6�   $��17(�6�   	� 5      -	   5        $" encrypt_File_AES_RadioButton��5   *   $��AES-Encryptie��6P   $��
41,30,50,0q�6h   $	��Top=�6|   $��Courier New)�6�   $
��Bold"�6�   $	��159��6�   $��17(�6�   5      5      �� 5      -
   5        $
 encrypt_File_Button��5      $��Encrypteren��6:   $
��Left=�6P   $��288,422,0,0q�6k   $	��Top=�6�   $	��300��6�   $��30��6�   �� 5      -   5        $, &encrypt_File_publicKey_RSA_Destination��5      $��Public Key ontvanger��6L   $
��Left=�6k   $��
10,264,0,0q�6�   $	��Top=�6�   $	��260��6�   $��30��6�   $��Courier New)�6�   $
��Bold"�6�   $��17(�6�   �� 5      -   5        $) #encrypt_File_Destination_PubKeyPath��5      $��Center=�6K   $��Center=�6o   $
��Left=�6�   $��30��6�   $��288,270,0,0q�6�   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6  $J	 DGeef de public-key op van de persoon die het bestand moet ontvangen.��6  �� 5      -   5        $6
 0encrypt_File_Destination_PubKeyPathLocatieButton��5      $	��...��6W   $
��Left=�6e   $��736,270,0,0q�6�   $	��Top=�6�   $��82��6�   $��30��6�   6�   �� 5      -   5        $&  encrypt_File_PrivKeyPathLocation��5      $��placeholder��6F   $
��Left=�6\   $��
10,312,0,0q�6w   $	��Top=�6�   $��Courier New)�6�   $
��Bold"�6�   $	��260��6�   $��17(�6�   �� 5      -   5        $- 'encrypt_File_PrivKeyPathLocationTextbox��5      $��Center=�6O   $��Center=�6s   $
��Left=�6�   $��29��6�   $��288,321,0,0q�6�   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6  $%	 Selecteer je eigen private-key.��6  �� 5      -   5        $,
 &encrypt_FIle_PrivKeyPathLocationButton��5      $	��...��6M   $
��Left=�6[   $��736,320,0,0q�6v   $	��Top=�6�   $��82��6�   $��30��6�   6�   5      5      �� 5      $��Decrypt File��6   .��5       � ��5!      �� 5"      $* $/Crypto_Program;component/BSFoto.jpg��6%   .+�5$      �� -   5        $! decrypt_File_DecryptieLabel��5$      $��Courier New)�6B   $
��Bold"�6[   $	��204��6m   $��17(�6y   $��Decryptie-Methode:��6�   $
��Left=�6�   $��
124,46,0,0q�6�   $	��Top=�6�   �� 5%      -   5        $$ decrypt_File_DecryptieGroubBox��5%   Y   $��Courier New)�6    $
��Bold"�69   $��17(�6K   $��	Decryptie��6�   $
��Left=�6�   $��
297,99,0,0q�6�   $	��Top=�6�   $��74��6�   $	��420��6�   .��5&      � $��	10,0,-2,0q�6   .+�5'      	� -   5        $" decrypt_File_DES_RadioButton��5'   *   $��DES-Decryptie��6P   $��
10,10,19,0q�6h   $	��Top=�6|   $��Courier New)�6�   $
��Bold"�6�   $	��159��6�   $��17(�6�   	� 5(      -   5        $" decrypt_File_AES_RadioButton��5(   *   $��AES-Decryptie��6P   $��116,28,125,0q�6h   $	��Top=�6~   $��Courier New)�6�   $
��Bold"�6�   $	��159��6�   $��17(�6�   5)      5*      �� 5+      -   5        $#
 decrypt_File_DecrypteerButton��5+      $��Decrypteren��6D   $
��Left=�6Z   $��297,506,0,0q�6u   $	��Top=�6�   $��30��6�   $	��300��6�   6�   �� 5-      -   5        $&  decrypt_File_geencrypteerd_Label��5-      $��Ge-encrypteerde file:��6F   $
��Left=�6f   $��
12,196,0,0q�6�   $	��Top=�6�   $	��260��6�   $��30��6�   $��Courier New)�6�   $
��Bold"�6�   $��17(�6�   �� 5.      -   5        $; 5decrypt_File_GeencrypteerdeFile_BestandLocatieTextBox��5.      $��Center=�6]   $��Center=�6�   $
��Left=�6�   $��30��6�   $��297,196,0,0q�6�   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6  $(	 "Selecteer de geëncrypteerde file.��6  �� 5/      -   5        $6
 0decrypt_File_GeencrypteerdeFile_FilePickerButton��5/      $	��...��6W   $
��Left=�6e   $��722,196,0,0q�6�   $	��Top=�6�   $��80��6�   $��30��6�   �� 51      -   5        $) #decrypt_File_SymmetricKeyLabel_Copy��51      $��symmetric key:��6I   $
��Left=�6b   $��
12,242,0,0q�6}   $	��Top=�6�   $	��260��6�   $��30��6�   $��Courier New)�6�   $
��Bold"�6�   $��17(�6�   �� 52      -   5        $5 /decrypt_File_SymmetricKey_BestandLocatieTextBox��52      $��Center=�6W   $��Center=�6{   $
��Left=�6�   $��30��6�   $��297,242,0,0q�6�   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6  $$	 Selecteer de Symmetrische Key.��6  �� 53      -   5        $0
 *decrypt_File_SymmetricKey_FilePickerButton��53      $	��...��6Q   $
��Left=�6_   $��722,242,0,0q�6z   $	��Top=�6�   $��80��6�   $��30��6�   �� 55      -   5        $% decrypt_File_EncryptedHash_Copy��55      $��Ge-encrypteerde Hash:��6E   $
��Left=�6e   $��
10,331,0,0q�6�   $	��Top=�6�   $	��260��6�   $��30��6�   $��Courier New)�6�   $
��Bold"�6�   $��17(�6�   �� 56      -   5        $6 0decrypt_File_EncryptedHash_BestandLocatieTextBox��56      $��Center=�6X   $��Center=�6|   $
��Left=�6�   $��30��6�   $��297,331,0,0q�6�   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6  $	 Selecteer de Hashfile��6  �� 57      -   5        $1
 +decrypt_File_EncryptedHash_FilePickerButton��57      $	��...��6R   $
��Left=�6`   $��722,331,0,0q�6{   $	��Top=�6�   $��80��6�   $��30��6�   �� 58      -   5        $ label��58      $��IV��6+   $
��Left=�68   $��
12,284,0,0q�6S   $	��Top=�6g   $	��260��6   $��Courier New)�6�   $
��Bold"�6�   $��17(�6�   $��30��6�   �� 59      -    5        $ decrypt_file_IVTextBox��59      $
��Left=�6>   $��Center=�6Y   $��Center=�6}   $��30��6�   $��297,284,0,0q�6�   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6�   $	 Selecteer de IV.��6   �� 5:      -!   5        $*
 $decrypt_file_IVBestandLocatieTextBox��5:      $	��...��6K   $
��Left=�6Y   $��722,284,0,0q�6t   $	��Top=�6�   $��80��6�   $��30��6�   �� 5;      -"   5        $% decrypt_file_PrivateRSAKeyLabel��5;      $��Private RSA key��6E   $
��Left=�6_   $��
12,382,0,0q�6z   $	��Top=�6�   $	��260��6�   $��30��6�   $��Courier New)�6�   $
��Bold"�6�   $��17(�6�   �� 5<      -#   5        $' !decrypt_file_PrivateRSAKeyTextBox��5<      $��Center=�6I   $��Center=�6m   $
��Left=�6�   $��30��6�   $��297,381,0,0q�6�   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6�   $	 Selecteer je Private-Key��6  �� 5=      -$   5        $&
  decrypt_file_PrivateRSAKeyButton��5=      $	��...��6G   $
��Left=�6U   $��722,381,0,0q�6p   $	��Top=�6�   $��80��6�   $��30��6�   6�   �� 5>      -%   5        $ public_rsa_Key��5>      $��placeholder��64   $
��Left=�6J   $��
12,426,0,0q�6e   $	��Top=�6y   $	��260��6�   $��30��6�   $��Courier New)�6�   $
��Bold"�6�   $��17(�6�   �� 5?      -&   5        $ decrypt_File_publicRsaKey��5?      $
��Left=�6A   $��Center=�6\   $��Center=�6�   $��30��6�   $��297,427,0,0q�6�   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6�   $.	 (Selecteer de public key van de verzender��6  �� 5@      -'   5        $%
 decrypt_File_PublicRsaKeyButton��5@      $
��....��6F   $
��Left=�6U   $��722,426,0,0q�6p   $	��Top=�6�   $��80��6�   $��30��6�   6�   5B      5C      �� 5D      $*��$Plain text encryptie - Steganografie��6   .��5E      � ��5F      �� 5G      $* $/Crypto_Program;component/BSFoto.jpg��6%   .+�5I      �� -(   5         ��Name$, &encrypt_plain_text_Steganografie_image��5I      $
��Left=�6L   $	��130��6g   $��
286,46,0,0q�6t   $	��Top=�6�   $	��278��6�   $��2��6�   �� 5J      -)   5        $��Kies een foto��5J      $
��Left=�65   $��286,198,0,0q�6P   $	��Top=�6e   $��30��6}   $	��300��6�   $��2��6�   �� 5K      -*   5        $# encrypt_steganografie_textbox��5K      $	��Top=�6E   $��Center=�6]   $	��206��6z   $	��695��6�   $��
99,254,0,0q�6�   $L��FVerander deze tekst naar de tekst die je in de foto wenst te verbergen��6�   $��2��6�   �� 5L      -+   5        $-��'encrypt_Steganografie_plain_text_Button��5L      $��Verberg Tekst��6L   $
��Left=�6d   $��286,511,0,0q�6   $	��Top=�6�   $	��300��6�   $��30��6�   $��2��6�   5M      5N      �� 5P      $*��$Plain text decryptie - Steganografie��6   .��5Q      � ��5R      �� 5S      $* $/Crypto_Program;component/BSFoto.jpg��6%   .+�5U      �� -,   5        $, &decrypt_plain_text_Steganografie_image��5U      $
��Left=�6L   $	��130��6g   $��
286,46,0,0q�6t   $	��Top=�6�   $	��294��6�   �� 5V      --   5        $��Kies een foto��5V      $
��Left=�65   $��286,197,0,0q�6P   $	��Top=�6e   $��30��6}   $	��300��6�   6�   �� 5W      -.   5         ��Name$% decrypt_steganografie_textblock��5W       
�������$	��Top=�6\   $��Center=�6t   $	��218��6�   $	��672��6�   $��112,250,0,0q�6�   $*��$Gedecrypteerde tekstoutput komt hier��6�   �� 5X      -/   5        $A
 ;decrypt_plain_text_Steganografie_BestandOmTeSteganograferen��5X      $��
Toon Tekst��6b   $
��Left=�6w   $��286,507,0,0q�6�   $	��Top=�6�   $	��300��6�   $��30��6�   6�   5Y      5Z      �� 5\      $%��Encrypt bestand - Steganografie��6   .��5]      � ��5^      �� 5_      $* $/Crypto_Program;component/BSFoto.jpg��6%   .+�5a      �� -0   5        $! encrypt_Steganografie_Image��5a      $
��Left=�6A   $	��144��6\   $��
275,34,0,0q�6i   $	��Top=�6}   $	��300��6�   �� 5b      -1   5        $,
 &encrypt_Steganografie_FotoPickerButton��5b      $��	Kies Foto��6M   $
��Left=�6a   $��275,216,0,0q�6|   $	��Top=�6�   $	��300��6�   $��30��6�   6�   �� 5c      -2   5        $( "encrypt_Steganografie_BestandLabel��5c      $��Te steganograferen file:��6H   $
��Left=�6k   $��
10,325,0,0q�6�   $	��Top=�6�   $	��260��6�   $��30��6�   $��Courier New)�6�   $
��Bold"�6�   $��17(�6�   �� 5d      -3   5        $* $encrypt_Steganografie_BestandTextBox��5d      $��Center=�6L   $��Center=�6p   $
��Left=�6�   $��30��6�   $��275,325,0,0q�6�   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6  �� 5e      -4   5        $"
 encrypt_Steganografie_Button��5e      $��Verberg Bestand��6C   $
��Left=�6]   $��275,450,0,0q�6x   $	��Top=�6�   $	��300��6�   $��30��6�   6�   �� 5f      -5   5        $6
 0encrypt_Steganografie_BestandOmTeSteganograferen��5f      $	��...��6W   $
��Left=�6e   $��722,325,0,0q�6�   $	��Top=�6�   $��80��6�   $��30��6�   6�   5g      5h      �� 5j      $%��Decrypt bestand - Steganografie��6   .��5k      � ��5l      �� 5m      $* $/Crypto_Program;component/BSFoto.jpg��6%   .+�5o      �� -6   5        $! decrypt_Steganografie_image��5o      $
��Left=�6A   $	��146��6\   $��
288,32,0,0q�6i   $	��Top=�6}   $	��300��6�   $��2��6�   �� 5p      -7   5        $,
 &decrypt_Steganografie_FotoPickerButton��5p      $��	Kies Foto��6M   $
��Left=�6a   $��288,202,0,0q�6|   $	��Top=�6�   $	��300��6�   $��30��6�   $��2��6�   �� 5q      -8   5        $( "decrypt_Steganografie_BestandLabel��5q      $��Opslag Pad:��6H   $
��Left=�6^   $��
23,322,0,0q�6y   $	��Top=�6�   $	��260��6�   $��30��6�   $��Courier New)�6�   $
��Bold"�6�   $��17(�6�   �� 5r      -9   5        $* $decrypt_Steganografie_BestandTextBox��5r      $��Center=�6L   $��Center=�6p   $
��Left=�6�   $��30��6�   $��288,322,0,0q�6�   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��420��6  $��2��6  �� 5s      -:   5        $"
 decrypt_Steganografie_Button��5s      $��Verhul Bestand��6C   $
��Left=�6\   $��288,444,0,0q�6w   $	��Top=�6�   $��30��6�   $	��300��6�   $��2��6�   �� 5t      -;   5        $6
 0decrypt_Steganografie_BestandOmTeSteganograferen��5t      $	��...��6W   $
��Left=�6e   $��713,322,0,0q�6�   $	��Top=�6�   $��80��6�   $��30��6�    J� RenderTransformOrigin$ 
0.406,0.55&�6  5u      5v      5w      o� 5x   
   -<   5         o�Name$
 menu��5x      $
��Left=�6   $��23��68   $	��Top=�6D   $	��851��6\   .@�5y      m� -=   5        $��bestand��5y      $��_Bestand��6&   .@�5z      m� ->   5        $��bestandMenuItem��5z      $��_Sluiten��62   6D   5{      m� 5|      -?   5        $
��Help��5|      $��_Help��6#   .@�5}      m� -@   5        $��algemeenHelpMenuItem��5}      $��	_Algemeen��67   6J   m� 5~      -A   5         m�Name$ EncryptieHelpMenuItem��5~      $��_Encrypteren��6:   .@�5      m� -B   5        $ EncryptFileHelp��5      $��_Encrypteer Bestand��68   6U   m� 5�      -C   5        $" EncryptFileSteganografieHelp��5�      $��Verberg Bestand��6E   6^   m� 5�      -D   5        $$ EncryptPlainTekstSteganografie��5�      $$��_Verberg Tekst - Steganografie��6G   6o   5�      m� 5�      -E   5        $ DecryptieHelpMenuItem��5�      $��_Decrypteren��6:   .@�5�      m� -F   5        $ DecryptFileHelp��5�      $��_Decrypteer Bestand��68   6U   m� 5�      -G   5        $" DecryptFileSteganografieHelp��5�      $3��-_Verborgen Bestand Terughalen - Steganografie��6E   6|   m� 5�      -H   5        $# DecryptTekstSteganografieHelp��5�      $1��+_Verborgen Tekst terughalen - Steganografie��6F   6{   5�      5�      5�      5�      5�      