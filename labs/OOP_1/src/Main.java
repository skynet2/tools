import java.io.IOException;

public class Main {

    public static void main(String[] args) throws IOException {
        BinWriter bw = new BinWriter("F:\\val.dat");
        bw.writeInt32(4);
        bw.close();
        BinReader br = new BinReader("F:\\val.dat");
        System.out.println(br.readInt32());
    }
}
